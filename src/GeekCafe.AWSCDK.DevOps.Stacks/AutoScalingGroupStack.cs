using System;
using System.Linq;
using Amazon.CDK;
using Amazon.CDK.AWS.AutoScaling;
using Amazon.CDK.AWS.EC2;
using GeekCafe.AWSCDK.DevOps.Configuration;

namespace GeekCafe.AWSCDK.DevOps.Stacks
{
    public class AutoScalingGroupStack: Stack
    {
        private IConfigSettings _config;
        public AutoScalingGroupStack(Construct scope, IConfigSettings config, IStackProps props = null) : base(scope, $"{config.Asg.StackName}", props)
        {
            _config = config;
        }

        public Amazon.CDK.AWS.AutoScaling.AutoScalingGroup Create(Amazon.CDK.AWS.EC2.Vpc vpc, SecurityGroup sg)
        {
            // todo define roles in config
            var role = new Security.Roles.IamRole().Create(this);
            var selection = new SubnetSelection
            {
                SubnetType = SubnetType.PUBLIC
            };

            var healchCheck = HealthCheck.Elb(new ElbHealthCheckOptions
            {
                Grace = Duration.Minutes(5)
            });

            var asg = new Amazon.CDK.AWS.AutoScaling.AutoScalingGroup(this, _config.Asg.Name, new AutoScalingGroupProps
            {
                AutoScalingGroupName = _config.Asg.Name,
                Vpc = vpc,
                // todo parse enums and pull from config
                InstanceType = InstanceType.Of(InstanceClass.BURSTABLE3, InstanceSize.MICRO),
                // get the linux two type otherwise it defaults to the older image
                // todo parse enums and pull from config
                MachineImage = new AmazonLinuxImage(new AmazonLinuxImageProps { Generation = AmazonLinuxGeneration.AMAZON_LINUX_2 }),
                AllowAllOutbound = _config.Asg.AllowAllOutbound,
                DesiredCapacity = _config.Asg.DesiredCapacity,
                MinCapacity = _config.Asg.MinCapacity,
                MaxCapacity = _config.Asg.MaxCapacity,
                
                KeyName = _config.Asg.KeyName,
                AssociatePublicIpAddress = _config.Asg.AssociatePublicIpAddress,
                VpcSubnets = selection,
                Role = role,
                UserData = GetUserData(_config.Asg.UserDataPath),
                HealthCheck = healchCheck,
                SecurityGroup = sg

            });

            Utilities.Tagging.Tag(asg, _config.Asg.Tags);
            Utilities.Tagging.Tag(asg, _config.Tags);

            //asg.ScaleOnCpuUtilization()
            

            return asg;
        }

        private UserData GetUserData(string path)
        {
            var ud = UserData.ForLinux();

            if (System.IO.Directory.Exists(path))
            {
                // parse the directory
                var files = System.IO.Directory.GetFiles(path, "*.sh").OrderBy(f => f);
                foreach(var file in files)
                {
                    var lines = System.IO.File.ReadAllLines(file);
                    ud.AddCommands(lines);
                }
            }
            else if (System.IO.File.Exists(path))
            {
                // parse one file
                var lines = System.IO.File.ReadAllLines(path);
                ud.AddCommands(lines);
            }

            return ud;
            
        }
    }
}
