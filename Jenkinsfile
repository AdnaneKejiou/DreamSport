pipeline {
    agent any

    environment {
        SONARQUBE = 'LocalSonarQube'  // The name you set for your SonarQube server in Jenkins
        SONAR_TOKEN = credentials('SONAR_TOKEN')  // This will securely fetch your SonarQube token
        DOCKER_CREDENTIALS = credentials('DOCKER_CREDENTIALS')  // Docker Hub credentials (username and password)
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

        stage('Build Frontend (Angular)') {
            steps {
                script {
                    sh '''
                    docker run --rm \
                        -v $(pwd):/app \
                        -w /app/projetPfa/angular \
                        node:20-alpine \
                        sh -c "npm install && npm run build"
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

                    // Push images for each custom-built service
                    sh """
                    # Tag and push custom service images
                    docker tag projetpfa-gestionsite \$DOCKER_CREDENTIALS_USR/gestionsite:latest
                    docker push \$DOCKER_CREDENTIALS_USR/gestionsite:latest

                    docker tag projetpfa-gestionemployer \$DOCKER_CREDENTIALS_USR/gestionemployer:latest
                    docker push \$DOCKER_CREDENTIALS_USR/gestionemployer:latest

                    docker tag projetpfa-auth \$DOCKER_CREDENTIALS_USR/auth:latest
                    docker push \$DOCKER_CREDENTIALS_USR/auth:latest

                    docker tag projetpfa-gestionutilisateur \$DOCKER_CREDENTIALS_USR/gestionutilisateur:latest
                    docker push \$DOCKER_CREDENTIALS_USR/gestionutilisateur:latest

                    docker tag projetpfa-gestionequipe \$DOCKER_CREDENTIALS_USR/gestionequipe:latest
                    docker push \$DOCKER_CREDENTIALS_USR/gestionequipe:latest

                    docker tag projetpfa-gestionreservation \$DOCKER_CREDENTIALS_USR/gestionreservation:latest
                    docker push \$DOCKER_CREDENTIALS_USR/gestionreservation:latest

                    docker tag projetpfa-servicemail \$DOCKER_CREDENTIALS_USR/servicemail:latest
                    docker push \$DOCKER_CREDENTIALS_USR/servicemail:latest

                    docker tag projetpfa-chatetinvitation \$DOCKER_CREDENTIALS_USR/chatetinvitation:latest
                    docker push \$DOCKER_CREDENTIALS_USR/chatetinvitation:latest

                    docker tag projetpfa-apigateway \$DOCKER_CREDENTIALS_USR/apigateway:latest
                    docker push \$DOCKER_CREDENTIALS_USR/apigateway:latest
                    """
                }
            }
        }

        stage('Deploy Application with Ansible') {
            steps {
                script {
                    sh '''
                    ansible-playbook -i deploy/inventory.ini deploy/deploy.yml --ask-become-pass
                    '''
                }
            }
        }
    }
}
