#####################################################################################################################################################
# install docker & configure
sudo amazon-linux-extras install docker -y
#start docker
sudo service docker start
# make sure it statys running
sudo chkconfig docker on
#Add the ec2-user to the docker group so you can execute Docker commands without using sudo.
sudo usermod -a -G docker ec2-user
#get the latest docker-compose program
sudo curl -L https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m) -o /usr/local/bin/docker-compose
#fix permissions
sudo chmod +x /usr/local/bin/docker-compose

# create a link to add docker-compose to the path
sudo ln -s /usr/local/bin/docker-compose /usr/bin/docker-compose

# / docker install
#####################################################################################################################################################
