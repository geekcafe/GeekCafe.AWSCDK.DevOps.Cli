#####################################################################################################################################################
# cloud watch agent
#
#
sudo wget https://s3.amazonaws.com/amazoncloudwatch-agent/amazon_linux/amd64/latest/amazon-cloudwatch-agent.rpm
# install the agent
sudo rpm -U ./amazon-cloudwatch-agent.rpm

# start it - it seems i need to start it one time before creating my config file below
# if i create the file first, it seems to get deleted on the intial start
sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -m ec2 -a start

# define the path
readonly readonly CLOUD_WATCH_AGENT_CONFIG_PATH="/opt/aws/amazon-cloudwatch-agent/etc"

# may need create the directory (the install or start should have done that)
#mkdir -p $CLOUD_WATCH_AGENT_CONFIG_PATH

# define the cloudwatch agent config file
readonly CLOUD_WATCH_AGENT_CONFIG_FILE_TEMP="${CLOUD_WATCH_AGENT_CONFIG_PATH}/temp-config.json"
readonly CLOUD_WATCH_AGENT_CONFIG_FILE="${CLOUD_WATCH_AGENT_CONFIG_PATH}/amazon-cloudwatch-agent.json"

echo "Making Primary Config File (temp and final path)"
echo "${CLOUD_WATCH_AGENT_CONFIG_FILE_TEMP}"
echo "${CLOUD_WATCH_AGENT_CONFIG_FILE}"



# define the contents of the file. use the temp file incase the main file gets wiped out
# __PROJECT_NAME__ will get replaced in the next section
sudo cat > "${CLOUD_WATCH_AGENT_CONFIG_FILE_TEMP}" << 'EOF'
{  
  "logs": {
    "logs_collected": {
      "files": {
        "collect_list": [
          {
            "file_path": "/opt/aws/amazon-cloudwatch-agent/logs/amazon-cloudwatch-agent.log",
            "log_group_name": "__PROJECT_NAME__/web/cloudwatch-agent",
            "log_stream_name": "{instance_id}",
            "timezone": "UTC"
          },
          {
            "file_path": "/var/log/__PROJECT_NAME__/*",
            "log_group_name": "__PROJECT_NAME__/web/app",
            "log_stream_name": "{instance_id}",
            "timezone": "UTC"
          }
        ]
      }
    },
    "log_stream_name": "my_log_stream_name",
    "force_flush_interval" : 15
  }
}
EOF



# this should be loaded as an environment variable, or you can set it here
if [ -z $PROJECT ]
then
 # not found so set the variable to one of your choosing
 export PROJECT="project_name_unknown"
 # write it out to the file to persist on reboots
 echo "export PROJECT=${PROJECT}" >> /etc/profile.d/export_instance_tags.sh
fi

# replace the __PROJECT_NAME__ with the variable $project value
sed "s/__PROJECT_NAME__/$PROJECT/g" "${CLOUD_WATCH_AGENT_CONFIG_FILE_TEMP}" > "${CLOUD_WATCH_AGENT_CONFIG_FILE}"

# restart it to load the new configuration
sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -m ec2 -a stop
sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -m ec2 -a start

# run a status check, so it will appear in the user-data logs
sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -m ec2 -a status

#
#
# / cloud watch agent
#####################################################################################################################################################
