using System;
using Microsoft.Extensions.CommandLineUtils;

namespace GeekCafe.AWSCDK.DevOps.Cli.Commands.FactoryItems
{
    
    public class CreateConfigTemplateCommand : BaseCommand, ICommandFactoryItem
    {
        public string Name => "template";
        public CommandLineApplication BuildCommand(CommandLineApplication command)
        {

            Register(command, "Deploy Infrastructure");

            var Path = command.Option("-d | --dir", $"The directory to create the template", CommandOptionType.SingleValue);


            command.OnExecute(() =>
            {
                if (!IsValid(new[] { Path })) return (int)ExitCodes.MISSING_OPTIONS;

                var path = Path.Value();

                Utils.Logger.Log($"Creating a template file in ${path}");
                // build the template
                var result = DevOps.Configuration.ConfigSettngs.CreateTemplate(path);


                Utils.Logger.Log($"Template file created ${result}");


                // return success
                return (int)ExitCodes.SUCCESS;
            });

            return command;
        }
    }
}
