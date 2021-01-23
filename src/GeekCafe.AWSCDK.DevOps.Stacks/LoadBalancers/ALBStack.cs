using System;
using Amazon.CDK;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Amazon.CDK.AWS.AutoScaling;
using Amazon.CDK.AWS.EC2;

namespace GeekCafe.AWSCDK.DevOps.Stacks.LoadBalancers
{
    public class ALBStack : Stack
    {

        public ALBStack(Construct scope, string id, string project, string environment, IStackProps props = null) : base(scope, $"{id}-alb-stack", props)
        {
        }

        public ApplicationLoadBalancer Create(Construct construct, Vpc vpc, AutoScalingGroup asg, SecurityGroup sg, string name)
        {
            var lb = new ApplicationLoadBalancer(construct, "LB", new ApplicationLoadBalancerProps
            {
                Vpc = vpc,
                InternetFacing = true,
                LoadBalancerName = name,
                SecurityGroup = sg

            });

            Amazon.CDK.Tags.Of(lb).Add("Name", $"{name}");

            // add a listener
            var listener = AddListener(lb, 80, null);
            var appPort = 80;
            var group = listener.AddTargets($"AppFleet", new AddApplicationTargetsProps
            {
                Port = appPort,
                Targets = new[] { asg }
            });

            // add specific tags
            Amazon.CDK.Tags.Of(listener).Add("Name", $"{name}-listner");
            Amazon.CDK.Tags.Of(group).Add("Name", $"{name}-fleet");


            // exmple of a fixed ok message returned by the LB
            listener.AddAction($"FixedOkMessage", new AddApplicationActionProps
            {
                Priority = 10,
                Conditions = new[] { ListenerCondition.PathPatterns(new[] { "/ok" }) },
                Action = ListenerAction.FixedResponse(200, new FixedResponseOptions
                {
                    ContentType = "text/html",
                    MessageBody = "OK"
                })
            });

            // example of a fixed health status message returned by LB
            listener.AddAction($"LBHealthInfo", new AddApplicationActionProps
            {
                Priority = 15,
                Conditions = new[] { ListenerCondition.PathPatterns(new[] { "/lb-status" }) },
                Action = ListenerAction.FixedResponse(200, new FixedResponseOptions
                {
                    ContentType = "application/json",
                    MessageBody = "{ \"lb\": { \"type\": \"application-load-balancer\", \"launchDateUtc\": \"{" + DateTime.UtcNow + "}\", \"status\": \"ok\" } }"
                })
            });

            // this id was obtained from the certificate manager
            // TODO, get the certificate (if it exists based on tags?)
            // or pull the cert from a configuration file and only add the cert if it exists
            var certArn = "arn:aws:acm:us-east-1:867915409343:certificate/eb2b584c-421d-4134-b679-1746642b5e3f";
            listener = AddListener(lb, 443, certArn);

            // forward any ssl requests to the target group
            listener.AddAction("SSLForward", new AddApplicationActionProps
            {
                Action = ListenerAction.Forward(new[] { group }),
            });



            return lb;
        }

        private ApplicationListener AddListener(ApplicationLoadBalancer lb, int port, string certArn = null)
        {

            var certs = (certArn == null) ? null : new ListenerCertificate[] { new ListenerCertificate(certArn) };


            var listener = lb.AddListener($"App2BalancerListener_{port}", new BaseApplicationListenerProps
            {
                Open = true,
                Certificates = certs,
                Port = port,
            });

            //listener.Connections.AllowDefaultPortFromAnyIpv4("Open to the world");

            return listener;


        }
    }
}
