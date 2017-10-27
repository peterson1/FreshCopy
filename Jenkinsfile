pipeline {
  agent any
  stages {
    stage('Clear Oldies') {
      steps {
        deleteDir()
      }
    }
    stage('Get Newbies') {
      steps {
        git(url: 'https://github.com/peterson1/FreshCopy.git', branch: 'master', changelog: true, credentialsId: 'peterson1')
      }
    }
    stage('Nuget Restore') {
      steps {
        bat(script: 'C:/Intel/nuget.exe restore FreshCopy.sln', returnStatus: true, returnStdout: true)
      }
    }
    stage('Build Projs') {
      steps {
        bat(script: '"\\"${tool \'MSBuild Tool 15\'}\\\\MSBuild.exe\\" FreshCopy.sln /p:Configuration=Release /p:Platform=\\"Any CPU\\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"', returnStatus: true, returnStdout: true)
      }
    }
  }
}