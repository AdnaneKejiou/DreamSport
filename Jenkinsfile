pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                git branch: 'docker-version',
                    url: 'https://gitlab.com/Aymen.Mechida/dreamsport.git',
                    credentialsId: 'gitlab-token'   
            }
        }

        stage('Unit Tests') {
            steps {
                agent {
                    docker {
                        image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    }
                }
                script {
                    sh 'dotnet test projetPfa/Auth/Auth.csproj'
                }
            }
        }
    }
    
}