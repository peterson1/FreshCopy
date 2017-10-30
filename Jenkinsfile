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
    stage('Run Tests') {
      parallel {
        stage('Batch 1') {
          steps {
            bat 'packages\\xunit.runner.console.2.3.1\\tools\\net452\\xunit.console FreshCopy.Tests\\bin\\Release\\FreshCopy.Tests.dll -trait "Batch=1" -parallel all -maxthreads unlimited'
          }
        }
        stage('Batch 2') {
          steps {
            bat 'packages\\xunit.runner.console.2.3.1\\tools\\net452\\xunit.console FreshCopy.Tests\\bin\\Release\\FreshCopy.Tests.dll -trait "Batch=2" -parallel all -maxthreads unlimited'
          }
        }
        stage('Batch 3') {
          steps {
            bat 'packages\\xunit.runner.console.2.3.1\\tools\\net452\\xunit.console FreshCopy.Tests\\bin\\Release\\FreshCopy.Tests.dll -trait "Batch=3" -parallel all -maxthreads unlimited'
          }
        }
        stage('Batch 4') {
          steps {
            bat 'packages\\xunit.runner.console.2.3.1\\tools\\net452\\xunit.console FreshCopy.Tests\\bin\\Release\\FreshCopy.Tests.dll -trait "Batch=4" -parallel all -maxthreads unlimited'
          }
        }
        stage('Batch 5') {
          steps {
            bat 'packages\\xunit.runner.console.2.3.1\\tools\\net452\\xunit.console FreshCopy.Tests\\bin\\Release\\FreshCopy.Tests.dll -trait "Batch=5" -parallel all -maxthreads unlimited'
          }
        }
      }
    }
    stage('Deploy to GDC') {
      environment {
        DEPLOY_DIR = 'B:\\deploy'
      }
      parallel {
        stage('Version Keeper') {
          steps {
            bat 'copy /Y FreshCopy.VersionKeeper.WPF\\bin\\Release\\FC.VersionKeeper.exe %DEPLOY_DIR%'
          }
        }
        stage('Update Checker') {
          steps {
            bat 'copy /Y FreshCopy.UpdateChecker.WPF\\bin\\Release\\FC.UpdateChecker.exe %DEPLOY_DIR%'
          }
        }
        stage('Server Control') {
          steps {
            bat 'copy /Y FreshCopy.ServerControl.WPF\\bin\\Release\\FC.ServerControl.exe %DEPLOY_DIR%'
          }
        }
      }
    }
  }
}