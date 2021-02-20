using System;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using GeekCafe.AWSCDK.DevOps.Configuration;

namespace GeekCafe.AWSCDK.DevOps.Stacks
{
    public class VpcStack : Stack
    {
        public Amazon.CDK.AWS.EC2.Vpc Vpc { get; private set; }

        public VpcStack(Construct scope, IConfigSettings config, IStackProps props = null) : base(scope, $"{config?.Vpc?.StackName}", props)
        {
            
            var vpcProps = new VpcProps
            {
                Cidr = (config.Vpc.Cidr != null) ? config.Vpc.Cidr : "10.0.0.0/16",
                NatGateways = config.Vpc.NatGateways,
                
            };

            // create the vpc
            Vpc = new Amazon.CDK.AWS.EC2.Vpc(this, config.Vpc.Name, vpcProps);

            // tag it
            Utilities.Tagging.Tag(Vpc, config.Vpc.Tags);
            Utilities.Tagging.Tag(Vpc, config.Tags);
            
        }
    }
}
