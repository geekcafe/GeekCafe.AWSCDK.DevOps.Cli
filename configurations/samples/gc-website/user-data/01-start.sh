#!/bin/bash -ex
# add user-data to a log file
exec > >(tee /var/log/user-data.log|logger -t user-data -s 2>/dev/console) 2>&1

# start time, which will also be used by the end.sh script
start_time="$(date -u +%s.%N)"

current_user=$(whoami)
echo "executing commands as user: ${current_user}"
whoami

sudo yum update -y

# using jq for parsing
sudo yum install -y jq

