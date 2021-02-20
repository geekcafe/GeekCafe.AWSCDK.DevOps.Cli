using System;
using Amazon.CDK;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Amazon.CDK.AWS.AutoScaling;
using Amazon.CDK.AWS.EC2;
using GeekCafe.AWSCDK.DevOps.Configuration;

namespace GeekCafe.AWSCDK.DevOps.Stacks.LoadBalancers
{
    public class ALBStack : Stack
    {
        private IConfigSettings _config;

        public ALBStack(Construct scope, IConfigSettings config, IStackProps props = null) : base(scope, $"{config.Alb.StackName}", props)
        {
            _config = config;
        }

        public ApplicationLoadBalancer Create(Construct construct, Amazon.CDK.AWS.EC2.Vpc vpc, Amazon.CDK.AWS.AutoScaling.AutoScalingGroup asg, SecurityGroup sg)
        {
            var lb = new ApplicationLoadBalancer(construct, _config.Alb.Name, new ApplicationLoadBalancerProps
            {
                Vpc = vpc,
                InternetFacing = true,
                LoadBalancerName = _config.Alb.Name,
                SecurityGroup = sg

            });

            Amazon.CDK.Tags.Of(lb).Add("Name", $"{_config.Alb.Name}");

            // add a listener
            var listener = AddListener(lb, 80, null);
            var appPort = 80;
            var group = listener.AddTargets($"AppFleet", new AddApplicationTargetsProps
            {
                Port = appPort,
                Targets = new[] { asg }
            });

            // add specific tags
            Amazon.CDK.Tags.Of(listener).Add("Name", $"{_config.Alb.Name}-listner");
            Amazon.CDK.Tags.Of(group).Add("Name", $"{_config.Alb.Name}-fleet");


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

            //"arn:aws:acm:us-east-1:xxxxxxxxx:certificate/eb2b584c-421d-4134-b679-1746642b5e3f"
            if (_config.Alb.CertArn != null)
            {
                listener = AddListener(lb, 443, _config.Alb.CertArn);

                // forward any ssl requests to the target group
                listener.AddAction("SSLForward", new AddApplicationActionProps
                {
                    Action = ListenerAction.Forward(new[] { group }),
                });
            }


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
            

            return listener;


        }
    }
}
