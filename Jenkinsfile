node('bddstack') {

    stage('code checkout') {
       echo "checking out source"
       checkout scm
    }

    stage('build') {
	    echo "Building..."
    }
 
    try {
	stage('validation') {
          dir('build'){
          }
	}
    } finally {
      //step([$class: 'JUnitResultArchiver', testResults: 'build/test-results/**/*.xml', healthScaleFactor: 1.0])
      junit 'build/test-results/**/*.xml'
      archiveArtifacts allowEmptyArchive: true, artifacts: 'build/test-results/**/*'
    }
}
