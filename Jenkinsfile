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

node('master') {
    stage('Test Arrays') {
       def myList = ['a', 'b', 'c']
           // prints 'a', 'b' and 'c'
           for (i in myList) {
             echo i
           }
       echo "each loop"    
       echo_all(myList)
       echo_via_for_loop(myList)       
    }
    stage('Test Secrets') {
        
        TEST_USERNAME = sh (
             script: 'oc get secret bdd-test-account -o yaml | grep username | awk -F ":" \'{print $2}\'',
              returnStdout: true).trim()		  
        TEST_PASSWORD = sh (
             script: 'oc get secret bdd-test-account -o yaml | grep password | awk -F ":" \'{print $2}\'',
              returnStdout: true).trim()
        echo "${TEST_PASSWORD}"
        echo "${TEST_USERNAME}"
        sh 'export TEST_USER=`echo "${TEST_USERNAME}"|base64 -d`\nexport TEST_PWD=`echo "${TEST_PASSWORD}"|base64 -d`\nenv'
    }
}
