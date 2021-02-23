



docker_dir="/app/docker"

#####################################################################################################################################################
# APP Setup

## Add application scripts here such as a docker-compose creation

sudo mkdir -p "${docker_dir}"
sudo cat > "${docker_dir}/docker-compose.yml" << 'EOF'
version: "3.7"
services:
  reverseproxy:
    image: nginx:alpine      
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf
    ports:
      - "80:80"
    restart: always
  web-service:    
    image: __DOCKER_IMAGE__     
    container_name: web-site
    volumes:
      - ${MEDIA_PATH}:/app/www/uploads
    environment:                 
      ENVIRONMENT : __ENVIRONMENT__
      APP_LOG_PATH: /app/log/
      ASPNETCORE_ENVIRONMENT: __ENVIRONMENT__
      ASPNETCORE_URLS: http://+:5000
      APP_DB_CONNECTION_STRING: ${APP_DB_CONNECTION_STRING}
      DB_HOST: ${DB_HOST}
      DB_PORT: ${DB_PORT}
      DB_USER: ${DB_USER}
      DB_PASSWORD: ${DB_PASSWORD}
      DB_NAME: ${DB_NAME}      
    ports:
       - "5000:5000"   
    depends_on:
       - reverseproxy    
    expose:
      - "5000"      
    restart: always
EOF


# replace the __PLACEHOLDERS__ with the variable 
# getting an issue with / as delimeter, switching to pipe
sudo sed -i "s|__DOCKER_IMAGE__|${DOCKER_IMAGE}|g" "${docker_dir}/docker-compose.yml"
sudo sed -i "s|__ENVIRONMENT__|${ENVIRONMENT}|g" "${docker_dir}/docker-compose.yml" 


### switch to the correct directory 
cd "${docker_dir}"
### launch it

### login to ecr
aws ecr get-login-password --region ${DOCKER_REPO_REGION} | docker login --username AWS --password-stdin ${DOCKER_REPO}

docker-compose up -d

# / App Setup
#####################################################################################################################################################