using System;
using Microsoft.Extensions.CommandLineUtils;

namespace GeekCafe.AWSCDK.DevOps.Cli.Commands.FactoryItems
{
    public class DeployCommand: BaseCommand, ICommandFactoryItem
    {
        public string Name => "deploy";
        public CommandLineApplication BuildCommand(CommandLineApplication command)
        {

            Register(command, "Create a configuration template");
            

            command.OnExecute(() =>
            {
                if (!IsValid()) return (int)ExitCodes.MISSING_OPTIONS;

                // execute the stack service
                

                // return success
                return (int)ExitCodes.SUCCESS;
            });

            return command;
        }
    }
}
