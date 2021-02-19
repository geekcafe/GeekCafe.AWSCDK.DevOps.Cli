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


            app.OnExecute(() =>
            {
                // get the empty command
                var cmd = new EmptyCommand(options);
                return cmd.Execute();

                // show the help if a matching command wasn't passed in
                //app.ShowHelp();
                //return (int)ExitCodes.MISSING_OPTIONS;
            });

            var exitCode = 0;
            try
            {
                // attempt to execute a command based on what we registred above
                Utils.Logger.Log($"{app.Name} starting exection.");
                exitCode = app.Execute(args);
                Utils.Logger.Log($"{app.Name} executed successfully.");
            }
            catch (Exception ex)
            {
                // something bad happened
                Utils.Logger.Log($"{app.Name} executed with errors.");
                Utils.Logger.Log($"Fatel Exception {ex.Message}");
                exitCode = (int)ExitCodes.FATEL_ERROR;
            }


            return exitCode;

            
        }

        

        //public static void LogArgs(string[] args)
        //{
        //    try
        //    {
        //        // writing a log file for the args
        //        var filePath = System.IO.Directory.GetCurrentDirectory();
        //        filePath = System.IO.Path.Join(filePath, "logs");
        //        System.IO.Directory.CreateDirectory(filePath);

        //        filePath = System.IO.Path.Join(filePath, $"args-{System.DateTime.UtcNow.Ticks.ToString()}.txt");
        //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
        //        {
        //            file.WriteLine("The following Args were found:");
        //            foreach (string arg in args)
        //            {
        //                file.WriteLine($"arg: {arg}");
        //            }

        //            file.WriteLine("-- end of args");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"exception: {ex.Message}");
        //    }
        //}

    }
}
