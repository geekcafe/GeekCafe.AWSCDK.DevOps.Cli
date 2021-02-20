#####################################################################################################################################################
# scripts to export EC2 instance tags to environment variables

# using jq for parsing
sudo yum install -y jq

# add boot script which loads environment variables
cat > /etc/profile.d/export_instance_tags.sh << 'EOF'
#!/bin/bash
# fetch instance info
INSTANCE_ID=$(curl -s http://169.254.169.254/latest/meta-data/instance-id)
REGION=$(curl -s http://169.254.169.254/latest/dynamic/instance-identity/document | jq -r .region)
# export instance tags
export_statement=$(aws ec2 describe-tags --region "$REGION" \
                        --filters "Name=resource-id,Values=$INSTANCE_ID" \
                        --query 'Tags[?!contains(Key, `:`)].[Key,Value]' \
                        --output text | \
                        awk  '{ 
                              x=""
                              for(i=1;i<=NF;i++) { 
                                if(i==1) {
                                  x="export " toupper($i)"=""\"";
                                }else {
                                  # add the space
                                  if(i>2) x=x" ";
                                  x=x$i;
                                } 
                                # close it off and print it
                                if(i==NF) { x=x"\""; print x; }      
                              }; 
                          }' 
                        )
eval $export_statement
# export instance info
export INSTANCE_ID
export REGION
EOF



# run the script
sudo chmod +x /etc/profile.d/export_instance_tags.sh

# reload the profile so that the environment variables are available here
source ~/.bash_profile

echo "Environment: ${ENVIRONMENT}"
echo "Bucket Name: ${BUCKET_NAME}"
echo "Project: ${PROJECT}"

# / scripts to export EC2 instance tags to environment variables
#####################################################################################################################################################