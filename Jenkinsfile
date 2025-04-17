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

                    // Push images using docker-compose to Docker Hub
                    sh 'docker compose -f projetPfa/docker-compose.yml push'
                }
            }
        }

        stage('Deploy Application with Ansible') {
            steps {
                withCredentials([
                    sshUserPrivateKey(credentialsId: 'jenkins-ssh-key', keyFileVariable: 'SSH_KEY'),
                    string(credentialsId: 'ansible-become-password', variable: 'ANSIBLE_BECOME_PASSWORD')
                ]) {
                    sh '''
                    export ANSIBLE_HOST_KEY_CHECKING=False

                    ansible-playbook -i deploy/inventory.ini deploy/deploy.yml \
                        --private-key "$SSH_KEY" \
                        --user aymen \
                        --become \
                        --extra-vars "ansible_become_password=$ANSIBLE_BECOME_PASSWORD"
                    '''
                }
            }
        }


    }
}
