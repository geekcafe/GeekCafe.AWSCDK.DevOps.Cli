#####################################################################################################################################################
# S3 Mounting
# install epel (a required package for 3sfs-fuse)
sudo amazon-linux-extras install epel -y
# install s3fs-fuse to mount s3 drives to our instance
sudo yum install s3fs-fuse -y
# make a directory for the media files, we're going to map to S3
if [ -z $S3_MEDIA_DIR ]
then
 # not found so set the variable to one of your choosing
 export S3_MEDIA_DIR="/app/s3/media"
 # write it out to the file to persist on reboots
 echo "export S3_MEDIA_DIR=${S3_MEDIA_DIR}" >> /etc/profile.d/export_instance_tags.sh
fi

sudo mkdir -p $S3_MEDIA_DIR

# sudo s3fs bucket-name local directory add the role, were using, allow the local directory to have files in it
# $ROLE_NAME is being pulled from an environment variable, which is loaded via the EC2 tags with a matching tag name
# and value contains the ROLE_NAME
sudo s3fs $BUCKET_NAME $S3_MEDIA_DIR -o iam_role=$ROLE_NAME -o allow_other -o nonempty
#sudo s3fs praxent-axiom-database /app/s3 -o iam_role=$ROLE_NAME -o allow_other -o nonempty
# / S3 Mounting
#####################################################################################################################################################