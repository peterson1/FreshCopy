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
        checkout scm
      }
    }
    stage('Nuget Restore') {
      steps {
        bat "\"${env.JENKINS_HOME}\\NuGet\\nuget.exe\" restore FreshCopy.sln"
      }
    }
    stage('Build Projs') {
      steps {
        bat "\"${tool 'MSBuild Tool 15'}\\MSBuild.exe\" FreshCopy.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
      }
    }
  }
}