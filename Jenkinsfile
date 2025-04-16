pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                git branch: 'docker-version', url: 'https://gitlab.com/Aymen.Mechida/dreamsport.git'
            }
        }

        stage('Unit Tests') {
            steps {
                script {
                    sh 'dotnet test projetpfa/auth/auth.csproj'
                }
            }
        }
    }
    
}