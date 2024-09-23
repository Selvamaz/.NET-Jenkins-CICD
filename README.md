# C# .NET Jenkins CICD
Jenkins CICD for C# .NET Project
<br>

> [!Note]
> **Tools Used : Github, Dotnet SDK 8.0, Jenkins, AWS S3 and Docker**


## Basic
1. Create Ubuntu 24 EC2 Server with SSH, HTTP and your favourite SG ports turned on
2. Install Jenkins, Docker, Git, zip, unzip and awscli
3. Install Dotnet SDK 8.0 from official microsoft linux terminal commands

## Stage 1 : Clone Repository
To retrieve the latest code from the specified GitHub repository
    ```stage('Clone Repository') {
            steps {
                git 'https://github.com/Selvamaz/.NET-Jenkins-CICD.git'
            }
        }``` 

## Stage 2 : Build
To restore dependencies and compile the application into an executable format. 
    ```stage('Build') {
            steps {
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release'
            }
        }``` 
dotnet restore: Downloads and installs any dependencies specified in the project files. This ensures that all required packages are available for the build.
dotnet build --configuration Release: Compiles the application in Release mode, optimizing it for production use. It generates the necessary binaries for the application.

## Stage 3 : Test
This command executes any tests defined in the project. Running tests in the CI/CD pipeline is crucial for maintaining code quality and catching issues early
    ```stage('Test') {
            steps {
                sh 'dotnet test --configuration Release'
            }
        }``` 

## Stage 4 : Package
To prepare the application for deployment by publishing the compiled output. 
dotnet publish: Packages the application and its dependencies into a folder for deployment. The -o out option specifies that the output should be placed in the out directory.
    ```stage('Package') {
            steps {
                sh 'dotnet publish -c Release -o out'
            }
        }``` 

## Stage 5 : Upload Artifcat to S3
Download AWS Credentials plugin and set up aws credentials in jenkins credentials (AWS User access keys)
Upload out folder to s3 bucket - Saving Artificatory
    ```stage('Upload to S3') {
            steps {
                script {
                    // Upload the published output to S3
                    withCredentials([[$class: 'AmazonWebServicesCredentialsBinding', credentialsId: 'AWS User']]) {
                        sh "aws s3 cp out s3://dotnetarchive --recursive"
                    }
                }
            }
        }``` 

## Stage 6 : Build Docker Images
To create a Docker image containing the application and its dependencies. Make sure Dockerfile is writted and set it to use out folder which has the artifacts/packages
    ```stage('Docker Build') {
            steps {
                script {
                    sh 'docker rmi -f $DOCKER_IMAGE || true'
                    sh 'docker build -t $DOCKER_IMAGE .'
                }
            }
        }``` 

## Stage 7 : Deploy
To run the application in a Docker container
    ```sstage('Deploy') {
            steps {
                script {
                    sh 'docker run -itd -p 8081:80 $DOCKER_IMAGE'
                }
            }
        }``` 


## Run 
Container will be running in the background. Copy the container id and use '''sudo docker logs container_id'''
It will display Hello World each five seconds
This is the result
