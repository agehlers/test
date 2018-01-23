@NonCPS // has to be NonCPS or the build breaks on the call to .each
def echo_all(list) {
    list.each { item ->
        echo "${item}"
    }
}

def echo_via_for_loop(list) {
    for (int i = 0; i < list.size(); i++) {
        sh "echo ${list[i]}"
    }
}

def APPNAME = 'devxp'

//
// Install plugins
// Priviledged operation, requires script approvalse for
// method hudson.model.UpdateCenter getPlugin java.lang.String
// method hudson.model.UpdateSite$Plugin deploy
// method jenkins.model.Jenkins getUpdateCenter
// staticMethod jenkins.model.Jenkins getInstance
// Under https://jenkins-<namespace>.pathfinder.gov.bc.ca/scriptApproval/
//       --> Signatures already approved:
//
// Jenkins.instance.updateCenter.getPlugin("htmlpublisher").deploy()

//See https://github.com/jenkinsci/kubernetes-plugin
podTemplate(label: 'owasp-zap', name: 'owasp-zap', serviceAccount: 'jenkins', cloud: 'openshift', containers: [
  containerTemplate(
    name: 'jnlp',
    image: '172.50.0.2:5000/openshift/jenkins-slave-zap',
    resourceRequestCpu: '500m',
    resourceLimitCpu: '1000m',
    resourceRequestMemory: '3Gi',
    resourceLimitMemory: '4Gi',
    workingDir: '/tmp',
    command: '',
    args: '${computer.jnlpmac} ${computer.name}'
  )
]) {
     node('owasp-zap') {
       sh('env')
     }
   }



node ('zap') {
    stage('ZAP Testing') {
           sh 'env'
           pwd
        }
    }

node('maven') {
    stage('Test Arrays') {
       def myList = ['a', 'b', 'c']
           // prints 'a', 'b' and 'c'
           for (i in myList) {
             echo i
           }
       echo "each loop"    
       echo_all(myList)
       echo_via_for_loop(myList)       

       TEST_USERNAME = sh (
             script: 'oc get secret bdd-test-account -o yaml | grep username | awk -F ":" \'{print $2}\'',
              returnStdout: true).trim()		  
       TEST_PASSWORD = sh (
             script: 'oc get secret bdd-test-account -o yaml | grep password | awk -F ":" \'{print $2}\'',
              returnStdout: true).trim()
        echo "${TEST_PASSWORD}"
        echo "${TEST_USERNAME}"
        withEnv(["TEST_PWD=${TEST_PASSWORD}","TEST_USER=${TEST_PASSWORD}"]) {
           sh 'env|grep TEST'
           sh 'echo "$TEST_USER" | base64 -d'
           sh 'echo "$TEST_PWD" | base64 -d'
        }
    }
}

node ('bddstack') {
    stage('Test Secrets') {
        
        TEST_USERNAME = sh (
             script: 'oc get secret bdd-test-account -o yaml | grep username | awk -F ":" \'{print $2}\'',
              returnStdout: true).trim()		  
        TEST_PASSWORD = sh (
             script: 'oc get secret bdd-test-account -o yaml | grep password | awk -F ":" \'{print $2}\'',
              returnStdout: true).trim()
        echo "${TEST_PASSWORD}"
        echo "${TEST_USERNAME}"
        withEnv(["TEST_PWD=${TEST_PASSWORD}","TEST_USER=${TEST_PASSWORD}"]) {
           sh 'env|grep TEST'
           sh 'echo "$TEST_USER" | base64 -d'
           sh 'echo "$TEST_PWD" | base64 -d'
        }
    }
}
