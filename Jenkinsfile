pipeline {
    agent any

    environment {
        SONARQUBE = 'LocalSonarQube'  // The name you set for your SonarQube server in Jenkins
        SONAR_TOKEN = credentials('sonar-password')  // The SonarQube token credential you created in Jenkins
    }

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
                    withSonarQubeEnv(SONARQUBE) {
                        sh '''
                        dotnet sonarscanner begin /k:"your_project_key" /d:sonar.login=$SONAR_TOKEN /d:sonar.host.url="http://localhost:9000"
                        dotnet build
                        dotnet sonarscanner end /d:sonar.login=$SONAR_TOKEN
                        '''
                    }
                }
            }
        }
    }
}
