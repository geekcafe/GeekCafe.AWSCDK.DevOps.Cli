using System;
using Microsoft.Extensions.CommandLineUtils;

namespace GeekCafe.AWSCDK.DevOps.Cli.Commands.FactoryItems
{
    public abstract class BaseCommand
    {
        public Options.Common Options = new Options.Common();
        private CommandLineApplication _command;
        

        public BaseCommand() { }

        /// <summary>
        /// Registers the Commandline Applicaiton along with some Base Options
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandDescription"></param>
        public void Register(CommandLineApplication command, string commandDescription)
        {
            _command = command;

            Options.Register(command, commandDescription);
        }

        /// <summary>
        /// Determines if this command is valid. Default logic requres a connection string.
        /// </summary>
        /// <param name="requiredOptions">Array of options, which a specific command can deam as required.</param>
        /// <returns>true or false</returns>
        public bool IsValid(CommandOption[] requiredOptions = null)
        {
            var valid = true;
            

            if (requiredOptions != null)
            {
                foreach (var value in requiredOptions)
                {
                    if (!value.HasValue())
                    {
                        Utils.Logger.Log($"Missing {value.Template} {value.Description}");
                    }
                }
            }

            return valid;
        }

        
    }
}
