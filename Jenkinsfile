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
      environment {
        P_CONFIG = "/p:Configuration=Release"
        P_PLATFORM = "/p:Platform=\"Any CPU\""
        P_VERSION = "/p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
      }
      steps {
        bat "\"${tool 'MSBuild Tool 15'}\\MSBuild.exe\" FreshCopy.sln %P_CONFIG% %P_PLATFORM% %P_VERSION%"
      }
    }
    stage('Run Tests') {
      environment {
        RUNNER = 'packages\\xunit.runner.console.2.3.1\\tools\\net452\\xunit.console'
        TEST_DLL = 'FreshCopy.Tests\\bin\\Release\\FreshCopy.Tests.dll'
        RUN_ARGS = '-parallel all -maxthreads unlimited -trait'
      }
      parallel {
        stage('Batch 1') {
          steps {
            bat '%RUNNER% %TEST_DLL% %RUN_ARGS% "Batch=1"'
          }
        }
        stage('Batch 2') {
          steps {
            bat '%RUNNER% %TEST_DLL% %RUN_ARGS% "Batch=2"'
          }
        }
        stage('Batch 3') {
          steps {
            bat '%RUNNER% %TEST_DLL% %RUN_ARGS% "Batch=3"'
          }
        }
        stage('Batch 4') {
          steps {
            bat '%RUNNER% %TEST_DLL% %RUN_ARGS% "Batch=4"'
          }
        }
        stage('Batch 5') {
          steps {
            bat '%RUNNER% %TEST_DLL% %RUN_ARGS% "Batch=5"'
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