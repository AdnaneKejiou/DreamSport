pipeline {
    agent any

    environment {
        SONARQUBE = 'LocalSonarQube'  // The name you set for your SonarQube server in Jenkins
        SONAR_TOKEN = credentials('SONAR_TOKEN')  // This will securely fetch your SonarQube token
        DOCKER_CREDENTIALS = credentials('DOCKER_CREDENTIALS')  // Docker Hub credentials (username and password)
        ANSIBLE_BECOME_PASS = credentials('ansible-sudo-password')
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'docker-version',
                    url: 'https://gitlab.com/Aymen.Mechida/dreamsport.git',
                    credentialsId: 'gitlab-token'
            }
        }

        stage('Build Backend Services') {
            steps {
                script {
                    sh '''
                    docker compose -f projetPfa/docker-compose.yml build
                    '''
                }
            }
        }

        stage('Push Docker Images to Docker Hub') {
            steps {
                script {
                    // Login to Docker Hub
                    sh """
                    echo \$DOCKER_CREDENTIALS_PSW | docker login -u \$DOCKER_CREDENTIALS_USR --password-stdin
                    """
                    
                    // Get the list of images to ensure the build was successful
                    sh 'docker images'

                    // Push images using docker-compose to Docker Hub
                    sh 'docker compose -f projetPfa/docker-compose.yml push'
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
                                export PATH="$PATH:/root/.dotnet/tools:$PATH" &&
                                dotnet-sonarscanner begin /k:"DreamSports" /d:sonar.login=$SONAR_TOKEN -d:sonar.host.url="http://172.17.0.1:9000" \
                                -d:sonar.projectBaseDir=/app/projetPfa &&  # Specify the project directory
                                dotnet build /app/projetPfa/Auth/Auth.csproj &&  # Specify the csproj file
                                dotnet-sonarscanner end /d:sonar.login=$SONAR_TOKEN
                            '
                        '''
                    }
                }
            }
        }

        stage('Deploy') {
            steps {
                sshagent(['jenkins-ssh-key']) {
                    sh '''
                        ansible-playbook -i deploy/inventory.ini deploy/deploy.yml \
                        --private-key /var/lib/jenkins/.ssh/id_ed25519 \
                        --user aymen --become --extra-vars "ansible_become_password=${ANSIBLE_BECOME_PASS}"
                    '''
                }
            }
        }

    }
}
