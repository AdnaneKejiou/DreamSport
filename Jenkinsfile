pipeline {
    agent any

    environment {
        SONARQUBE = 'LocalSonarQube'  // The name you set for your SonarQube server in Jenkins
        SONAR_TOKEN = credentials('SONAR_TOKEN')  // This will securely fetch your SonarQube token
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
                    withSonarQubeEnv('LocalSonarQube') {
                        sh '''
                        docker run --rm \
                            -v $(pwd):/app \
                            -w /app \
                            -e SONAR_TOKEN=$SONAR_TOKEN \
                            mcr.microsoft.com/dotnet/sdk:8.0 \
                            sh -c '
                                dotnet tool install --global dotnet-sonarscanner &&
                                export PATH="$PATH:/root/.dotnet/tools" &&
                                /root/.dotnet/tools/dotnet-sonarscanner begin /k:"DreamSports" /d:sonar.login=$SONAR_TOKEN /d:sonar.host.url="http://host.docker.internal:9000" &&
                                dotnet build &&
                                /root/.dotnet/tools/dotnet-sonarscanner end /d:sonar.login=$SONAR_TOKEN
                            '
                        '''
                    }
                }
            }
        }



    }
}
