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
