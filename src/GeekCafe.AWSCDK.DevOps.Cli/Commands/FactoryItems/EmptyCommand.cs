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

            // execute the stack service
            var app = new App();

            // todo get from args
            var environment = Options.Environment;
            var project = Options.Project;
            var id = Options.Id;


            // create a vpc
            var vpcStack = new Stacks.VpcStack(app, id);

            ApplyDefaultTags(vpcStack, environment, project);

            // create the 

            // create a db
            var dbInstance = new Stacks.DataStorage.Databases.RDSDatabases.MySQLStack(app, $"id-mysql-stack-{id}");
            dbInstance.Create(vpcStack.Vpc, null, id);

            // create 


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
