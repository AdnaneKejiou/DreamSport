sonarQubeEnv = 'LocalSonarQube'

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
                script {
                    sh '''
                    docker run --rm \
                        -v $(pwd):/app \
                        -w /app \
                        mcr.microsoft.com/dotnet/sdk:8.0 \
                        dotnet test projetPfa/Auth/Auth.csproj
                    '''
                }
            }
        }

        stage('SonarQube Analysis') {
            steps {
                script {
                    // Run SonarQube analysis
                    withSonarQubeEnv(sonarQubeEnv) {
                        sh 'mvn clean install sonar:sonar'
                    }
                }
            }
        }
    }
}
