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
                Utils.Logger.Log($"ERROR Expected Config File information", Microsoft.Extensions.Logging.LogLevel.Critical);
                return -1;
            }

            Utils.Logger.Log($"Vpc Id: {config.Vpc.Id}");
            Utils.Logger.Log($"Vpc Name: {config.Vpc.Name}");
            Utils.Logger.Log($"Vpc Stack Id: {config.Vpc.StackName}");

            // create a vpc
            var vpcStack = new Stacks.VpcStack(app, config);
            ApplyDefaultTags(vpcStack, config.Environment, config.Project);

            

            // todo add db back into the process
            // create a db
            //var dbStack = new Stacks.DataStorage.Databases.RDSDatabases.MySQLStack(app, $"{id}", config);
            //var dbAccessSg = Stacks.Security.SecurityGroups.GenericSecurityGroup.BuildGenericSecurityGroup(dbStack, vpcStack.Vpc, "rds-access");
            //dbStack.Create(vpcStack.Vpc, dbAccessSg, id);

            // apply the tags 
            //ApplyDefaultTags(dbStack, environment, project);



            // create the autoscaling groupd
            var auto = new Stacks.AutoScalingGroupStack(app, config);
            //ApplyASGTags(auto);
            var sg = Stacks.Security.SecurityGroups.CreateHostSG(auto, vpcStack.Vpc, "web-app-asg-sg", "web application asg");
            var asg = auto.Create(vpcStack.Vpc, sg);
            



            var lb = new Stacks.LoadBalancers.ALBStack(app, config);            
            var worldToElb = Stacks.Security.SecurityGroups.CreateHttpHttps(lb, vpcStack.Vpc, "World To ELB", "ELB Access");
            
            // allow access from the ELB to the ASG
            asg.AddSecurityGroup(Stacks.Security.SecurityGroups.CreateHttpHttps(auto, vpcStack.Vpc, "ELB to ASG", "Access to ASG from the ELB", new[] { worldToElb }));

            lb.Create(lb, vpcStack.Vpc, asg, worldToElb);

            


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

        //private static void ApplyASGTags(IConstruct construct)
        //{
        //    Tags.Of(construct).Add("ENVIRONMENT", "dev");
        //    Tags.Of(construct).Add("PROJECT", "geek-cafe-web-site");
        //    Tags.Of(construct).Add("CERT_ARN", "arn:aws:acm:us-east-1:867915409343:certificate/eb2b584c-421d-4134-b679-1746642b5e3f");
        //    Tags.Of(construct).Add("BUCKET_NAME", "geekcafe-piranha-dev-media");
        //    Tags.Of(construct).Add("DOCKER_IMAGE", "867915409343.dkr.ecr.us-east-1.amazonaws.com/geekcafe-website:dev");
            
        //}
    }
}
