node {
	stage 'Checkout'
		checkout scm

	stage 'Build'
		bat 'C:/Intel/nuget.exe restore FreshCopy.sln'
		bat "\"${tool 'MSBuild Tool 15'}\" FreshCopy.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"

}