using System;
using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;

namespace GeekCafe.AWSCDK.DevOps.Cli.Commands.Options
{
    public class Common
    {
        
        private CommandOption EnvironmentArg { get; set; }
        private CommandOption ProjectArg { get; set; }
        private CommandOption UserDataArgs { get; set; }
        private CommandOption StackArgs { get; set; }
        private CommandOption IdArg { get; set; }


        public Common()
        {
        }

        public void Register(CommandLineApplication command, string commandDescription)
        {
            
            EnvironmentArg = command.Option("-e | --env", $"The environment", CommandOptionType.SingleValue);
            ProjectArg = command.Option("-p | --project", $"The project", CommandOptionType.SingleValue);
            IdArg = command.Option("-i | --id", $"The stack id", CommandOptionType.SingleValue);

            UserDataArgs = command.Option("-u | --user-data", $"User Data Scripts.  Add one or more.  The files will be appended", CommandOptionType.MultipleValue);
            StackArgs = command.Option("-s | --stack", $"One or more stack to execute", CommandOptionType.MultipleValue);

            command.Description = commandDescription;

            // help option
            command.HelpOption("-?|-h|--help");
        }

        /// <summary>
        /// Get the Environment Argument Value (defaults to dev)
        /// </summary>
        public string Environment
        {
            get
            {
                return EnvironmentArg.HasValue() ? EnvironmentArg.Value() : "dev";
            }
        }

        /// <summary>
        /// Get the Project Argument Value (defaults to project)
        /// </summary>
        public string Project
        {
            get
            {
                return ProjectArg.HasValue() ? ProjectArg.Value() : "project";
            }
        }

        /// <summary>
        /// Get the Stack Id Value (defaults to the values in Environment & Value with format of environment-project)
        /// </summary>
        public string Id
        {
            get
            {
                return IdArg.HasValue() ? IdArg.Value() : $"{Environment}-{Project}";
            }
        }

        /// <summary>
        /// A list of files that should be used in the User Data section
        /// </summary>
        public List<string> UserDataFiles
        {
            get
            {
                return UserDataArgs.HasValue() ? ProjectArg.Values : new List<string>();
            }
        }

        /// <summary>
        /// A list of Stacks to deploy
        /// </summary>
        public List<string> Stacks
        {
            get
            {
                return StackArgs.HasValue() ? StackArgs.Values : new List<string>();
            }
        }

    }
}
