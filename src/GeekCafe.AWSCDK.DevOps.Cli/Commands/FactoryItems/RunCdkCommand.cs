using System;
using System.Diagnostics;
using Microsoft.Extensions.CommandLineUtils;

namespace GeekCafe.AWSCDK.DevOps.Cli.Commands.FactoryItems
{
    
    public class RunCdkCommand : BaseCommand, ICommandFactoryItem
    {
        public string Name => "run";
        public CommandLineApplication BuildCommand(CommandLineApplication command)
        {

            Register(command, "Run the cdk");
            command.OnExecute(() =>
            {
                if (!IsValid()) return (int)ExitCodes.MISSING_OPTIONS;

                // execute the stack service

                //var escapedArgs = cmd.Replace("\"", "\\\"");
                var dir = System.IO.Directory.GetCurrentDirectory();

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        //Arguments = $"-c \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };
                process.Start();
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                Utils.Logger.Log(result);

                // return success
                return (int)ExitCodes.SUCCESS;
            });

            return command;
        }       
    }
}
