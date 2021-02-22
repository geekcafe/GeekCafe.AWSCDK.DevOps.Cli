using System;
using Amazon.CDK;
using Microsoft.Extensions.CommandLineUtils;

namespace GeekCafe.AWSCDK.DevOps.Cli.Commands.FactoryItems
{
   
    public class EmptyCommand : BaseCommand, ICommandFactoryItem
    {
        public EmptyCommand() { }
        public EmptyCommand(Options.Common options)
        {
            base.Options = options;
        }

        public string Name => "";
        public CommandLineApplication BuildCommand(CommandLineApplication command)
        {

            Register(command, "");
            command.OnExecute(() =>
            {
                return Execute();
            });

            return command;
        }

        public int Execute()
        {
            if (!IsValid()) return (int)ExitCodes.MISSING_OPTIONS;
                       
            var app = new App();                       
            var config = Options.ConfigSettngs;
           
            if(config == null)
            {
                Utilities.Logger.Log($"ERROR Expected Config File information", Microsoft.Extensions.Logging.LogLevel.Critical);
                return -1;
            }


            Utilities.Logger.Log($"Vpc Name: {config.Vpc.Name}");
            Utilities.Logger.Log($"Vpc Stack Id: {config.Vpc.StackName}");
            Utilities.Logger.Log($"Vpc Stack Id: {config.Vpc.Cidr}");

            // create a vpc
            var vpcStack = new Stacks.VpcStack(app, config);
            ApplyDefaultTags(vpcStack, config.Environment, config.Project);

            // consolidated security groups to avoid cicular references
            var dbSecurityGroup = Stacks.Security.SecurityGroups.CreateHostSG(vpcStack, vpcStack.Vpc, "RDS Security Group", "RDS Security Container");
            var asgSecurityGroup = Stacks.Security.SecurityGroups.CreateHostSG(vpcStack, vpcStack.Vpc, "web-app-asg-sg", "web application asg");
            var worldToElbSecurityGroup = Stacks.Security.SecurityGroups.CreateHttpHttps(vpcStack, vpcStack.Vpc, "World To ELB", "ELB Access");

            Stacks.Security.SecurityGroups.AddResourceAccessToRds(dbSecurityGroup, asgSecurityGroup);

            

            // database stack            
            var dbStack = new Stacks.DataStorage.Databases.RDSDatabases.MySQLStack(app, config.Rds.StackName);            
            dbStack.Create(vpcStack.Vpc, config, new[] { dbSecurityGroup });
                       

            // autoscaling group
            var autoScalingGroup = new Stacks.AutoScalingGroupStack(app, config);
            
            // todo this is only for testing, move this information to parameter store!!!            
            autoScalingGroup.AddTag(autoScalingGroup, "DB_HOST", dbStack.DBInstance.DbInstanceEndpointAddress);
            autoScalingGroup.AddTag(autoScalingGroup, "DB_PORT", dbStack.DBInstance.DbInstanceEndpointPort);
            autoScalingGroup.AddTag(autoScalingGroup, "DB_NAME", config.Rds.DatabaseName);
            autoScalingGroup.AddTag(autoScalingGroup, "DB_USER", config.Rds.UserName);
            autoScalingGroup.AddTag(autoScalingGroup, "DB_PASSWORD", config.Rds.Password);

            var asg = autoScalingGroup.Create(vpcStack.Vpc, asgSecurityGroup);
            
            // load balancer
            var lb = new Stacks.LoadBalancers.ALBStack(app, config);                                   
            lb.Create(lb, vpcStack.Vpc, asg, worldToElbSecurityGroup);

            
            var cloudAssembly = app.Synth();

            // return success
            return (int)ExitCodes.SUCCESS;
        }

        

        private static void ApplyDefaultTags(IConstruct construct, string environment, string project)
        {
            Tags.Of(construct).Add("Stack-Author", "Eric Wilson");
            Tags.Of(construct).Add("Stack-Version", "0.0.1-beta");
            Tags.Of(construct).Add("Stack-Tool", "GeekCafe-AWS-CDK");
            Tags.Of(construct).Add("Stack-Environment", environment);
            Tags.Of(construct).Add("Stack-Project", project);

        }
       
    }
}
