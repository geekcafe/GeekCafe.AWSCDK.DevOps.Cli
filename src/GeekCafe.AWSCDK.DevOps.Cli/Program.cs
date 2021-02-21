using System;
using Amazon.CDK;
using GeekCafe.AWSCDK.DevOps.Cli.Commands;
using GeekCafe.AWSCDK.DevOps.Cli.Commands.FactoryItems;
using Microsoft.Extensions.CommandLineUtils;

namespace GeekCafe.AWSCDK.DevOps.Cli
{
    sealed class Program
    {
        public static int Main(string[] args)
        {


            var app = new CommandLineApplication
            {
                Name = "Geek Cafe AWS CDK DevOps CLI",
                Description = "Wrapper for AWS CDK"
            };

            var options = new Commands.Options.Common();
            options.Register(app, "");

            // register the commands
            app.RegisterCommands();

            // 
            app.OnExecute(() =>
            {
                // if we get here then a registered command wasn't found
                // so we can run the default
                var cmd = new EmptyCommand(options);
                var result = cmd.Execute();
               
                if (result == (int)ExitCodes.MISSING_OPTIONS)
                {
                    app.ShowHelp();
                }

                return result;
                
            });

            var exitCode = 0;
            try
            {
                // attempt to execute a command based on what we registred above
                Utilities.Logger.Log($"{app.Name} starting exection.");
                exitCode = app.Execute(args);
                Utilities.Logger.Log($"{app.Name} executed successfully.");
            }
            catch (Exception ex)
            {
                // something bad happened
                Utilities.Logger.Log($"{app.Name} executed with errors.");
                Utilities.Logger.Log($"Fatel Exception {ex.Message}");
                exitCode = (int)ExitCodes.FATEL_ERROR;
            }

            return exitCode;            
        }
    }
}
