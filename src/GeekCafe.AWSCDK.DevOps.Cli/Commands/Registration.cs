using System;
using Microsoft.Extensions.CommandLineUtils;

namespace GeekCafe.AWSCDK.DevOps.Cli.Commands
{
    public static class Registration
    {
        /// <summary>
        /// Register the available commands
        /// </summary>
        /// <param name="app"></param>
        public static void RegisterCommands(this CommandLineApplication app)
        {
            // load the factory
            var factory = new CommandFactory();
                      
            // go through each item that is part of the factory
            foreach (var cmd in factory.Commands)
            {
                // register the items command and it's call back here
                // the system will only execute them based on the command name (cmd.Name)
                app.Command(cmd.Name, commandApp => { cmd.BuildCommand(commandApp); });
            }

        }
    }
}
