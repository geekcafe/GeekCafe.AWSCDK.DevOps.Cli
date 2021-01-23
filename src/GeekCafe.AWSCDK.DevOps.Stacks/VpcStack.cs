using System;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;

namespace GeekCafe.AWSCDK.DevOps.Stacks
{
    public class VpcStack : Stack
    {
        public Vpc Vpc { get; private set; }

        public VpcStack(Construct scope, string id, string project, string environment, IStackProps props = null) : base(scope, $"{id}-vpc-stack", props)
        {
            // The code that defines your stack goes here            
            // get the configuration from a file
            var config = new VpcProps
            {
                Cidr = "10.0.0.0/16",
                NatGateways = 0,

            };

            // create the vpc
            Vpc = new Vpc(this, "VPC", config);

            Amazon.CDK.Tags.Of(Vpc).Add("Name", $"{id}-vpc");
        }
    }
}
