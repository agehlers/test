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
           sh 'echo "$TEST_PASS" | base64 -d'
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
