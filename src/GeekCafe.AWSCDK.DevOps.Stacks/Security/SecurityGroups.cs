using System;
using Amazon.CDK.AWS.EC2;

namespace GeekCafe.AWSCDK.DevOps.Stacks.Security
{
    public class SecurityGroups
    {
        public SecurityGroups()  {  }

        public static SecurityGroup CreateHttpHttps(Constructs.Construct construct, Vpc vpc, string name, string description, IPeer[] peers = null)
        {
           
            // create the security groups
            var sg =  CreateHostSG(construct, vpc, name, description);

            // add the rules
            if(peers == null || peers.Length == 0)
            {
                var source = Peer.AnyIpv4();
                peers = new[] { source };
            }
            
            foreach(var peer in peers)
            {
                var http = new Port(Utils.Ports.GetPortProps(80, 80, "http"));
                var https = new Port(Utils.Ports.GetPortProps(443, 443, "https"));
                sg.AddIngressRule(peer, http, "ipv4-http");
                sg.AddIngressRule(peer, https, "ipv4-https");
            }
           
            return sg;
        }

        //public static SecurityGroup CreateWorldToElb(Constructs.Construct construct, Vpc vpc, string name, string description = null, IPeer[] peers = null)
        //{
        //    description = (description != null) ? description : "World To ELB";
        //    return CreateHttpHttps(construct, vpc, name, description, peers);
        //}

        //public static SecurityGroup CreateElbToHost(Constructs.Construct construct, Vpc vpc, string name, string description, IPeer elbSecurityGroup)
        //{
        //    description = (description != null) ? description : "ELB To Host";
        //    return CreateHttpHttps(construct, vpc, name, description, new [] { elbSecurityGroup});
        //}

        public static SecurityGroup CreateHostToRDS(Constructs.Construct construct, Vpc vpc, string name, string description, SecurityGroup elbSecurityGroup)
        {
            var ports = new Port[]
            {
                new Port(Utils.Ports.GetPortProps(3306, 3306, "MySQL/Auora"))
            };
            var sg = CreatePeer(construct, vpc, name, description, elbSecurityGroup, ports);

            return sg;
        }

        public static void AddResourceAccessToRds(SecurityGroup securityGroup, SecurityGroup resource)
        {
            var ports = new Port[]
            {
                new Port(Utils.Ports.GetPortProps(3306, 3306, "MySQL/Auora"))
            };

            foreach (var port in ports)
            {
                securityGroup.AddIngressRule(resource, port);
            }
        }

        public static SecurityGroup CreateHostSG(Constructs.Construct construct, Vpc vpc, string name, string description)
        {
            var props = new SecurityGroupProps
            {                
                Vpc = vpc,
                Description = description,
                SecurityGroupName = $"{name}",
            };

            // create some security groups
            var sg = new SecurityGroup(construct, name, props);

            Amazon.CDK.Tags.Of(sg).Add("Name", $"{name}");

            return sg;
        }

        public static SecurityGroup CreatePeer(Constructs.Construct construct, Vpc vpc, string name, string description, SecurityGroup peer, Port[] ports)
        {
            var sg = CreateHostSG(construct, vpc, name, description);

            foreach(var port in ports)
            {
                sg.AddIngressRule(peer, port);
            }
            
            return sg;
        }

     }
}



