using System;
using Amazon.CDK.AWS.EC2;

namespace GeekCafe.AWSCDK.DevOps.Stacks.Security.Utils
{
    public class Ports
    {
        public Ports()
        {
        }

        public static PortProps GetPortProps(int from, int to, string description)
        {
            var portProps = new PortProps
            {
                FromPort = from,
                ToPort = to,
                Protocol = Protocol.TCP,
                StringRepresentation = description
            };

            return portProps;
        }
    }
}
