pipeline {
    agent any

    environment {
        SONARQUBE = 'LocalSonarQube'  // The name you set for your SonarQube server in Jenkins
        SONAR_TOKEN = credentials('SONAR_TOKEN')  // This will securely fetch your SonarQube token
        DOCKER_USER = credentials('mechaymen')  // Docker Hub username
        DOCKER_PASS = credentials('adminadmin')  // Docker Hub password
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
                    sh '''
                    docker login -u $DOCKER_USER -p $DOCKER_PASS
                    docker tag your-backend-image $DOCKER_USER/your-backend-image:latest
                    docker push $DOCKER_USER/your-backend-image:latest
                    '''
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
