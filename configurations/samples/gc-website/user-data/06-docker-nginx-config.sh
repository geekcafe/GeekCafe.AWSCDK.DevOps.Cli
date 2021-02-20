
sudo mkdir -p /app/docker

sudo cat > /app/docker/nginx.conf << 'EOF'
# typically 1 per core
# determine cores 
# --- linux: grep processor /proc/cpuinfo | wc -l
# --- mac:  sysctl -n hw.ncpu
worker_processes 1;

error_log  /var/log/nginx/error.log  info;

events { 
    # check limit
    # --- mac/linux: ulimit -n
    worker_connections 1024; 
}

http {

    sendfile on;
    proxy_buffer_size   128k;
    proxy_buffers   4 256k;
    proxy_busy_buffers_size   256k;
    large_client_header_buffers 4 16k; 

    upstream app_servers {
        # this must be the same name as the docker container name
        # it's how it maps to the service
        # unless its in ECS then it's localhost            
        server web-service:5000;      

        # additional services can be added here
    }

    server {
        listen 80;
        #listen [::]:80;
        server_name $hostname;
        client_max_body_size 10M;                   # upload size
    
        # handle all requests (add additonal locations if needed)
        location / {
            proxy_pass         http://app_servers;  # references the upstream app_servers above
            proxy_redirect     off;           
            proxy_set_header   Connection keep-alive;
            proxy_set_header   Host $host;
            proxy_set_header   X-Real-IP $remote_addr;
            proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;            
            proxy_set_header   X-Forwarded-Host $server_name;
            proxy_set_header   X-NginX-Proxy true;                        
        }
    }
}

EOF