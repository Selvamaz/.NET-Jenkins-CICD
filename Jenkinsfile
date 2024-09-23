pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOCKER_IMAGE = 'myapp:latest'
    }

    stages {
        stage('Clone Repository') {
            steps {
                git 'https://github.com/Selvamaz/.NET-Jenkins-CICD.git'
            }
        }
        stage('Build') {
            steps {
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release'
            }
        }
        stage('Test') {
            steps {
                sh 'dotnet test --configuration Release'
            }
        }
        stage('Package') {
            steps {
                sh 'dotnet publish -c Release -o out'
            }
        }
        stage('Upload to S3') {
            steps {
                script {
                    // Upload the published output to S3
                    withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'AWS User']]) {
                        sh "aws s3 cp out s3://dotnetarchive --recursive"
                    }
                }
            }
        }
        stage('Docker Build') {
            steps {
                script {
                    sh 'docker rmi -f $DOCKER_IMAGE || true'
                    sh 'docker build -t $DOCKER_IMAGE .'
                }
            }
        }
        stage('Deploy') {
            steps {
                script {
                    sh 'docker run -itd -p 8081:80 $DOCKER_IMAGE'
                }
            }
        }
    }
}
