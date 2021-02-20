using System;
using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;

namespace GeekCafe.AWSCDK.DevOps.Cli.Commands.Options
{
    public class Common
    {
        
        private CommandOption _environmentArg { get; set; }
        private CommandOption _projectArg { get; set; }
        private CommandOption _userDataArgs { get; set; }
        private CommandOption _stackArgs { get; set; }
        private CommandOption _idArg { get; set; }
        private CommandOption _configurationFileArgs { get; set; }

        private Configuration.ConfigSettngs _configSettngs { get; set; }

        public Common() { }
        
        public void Register(CommandLineApplication command, string commandDescription)
        {

            _environmentArg = command.Option("-e | --env", $"The environment", CommandOptionType.SingleValue);
            // the -p command can interfer with the dotnet run command -p when pointing to a project
            // i may want to using something else like -x
            _projectArg = command.Option("-p | --project", $"The project", CommandOptionType.SingleValue);
            _idArg = command.Option("-i | --id", $"The stack id. If not supplied it will use Environment-Project as the Id", CommandOptionType.SingleValue);

            _userDataArgs = command.Option("-u | --user-data", $"User Data Scripts.  Add one or more.  The files will be appended", CommandOptionType.MultipleValue);
            _stackArgs = command.Option("-s | --stack", $"One or more stack to execute", CommandOptionType.MultipleValue);
            _configurationFileArgs = command.Option("-c | --c", $"Configuration File Path", CommandOptionType.MultipleValue);           
            // help option
            command.HelpOption("-?|-h|--help");

            command.Description = commandDescription;
        }

        /// <summary>
        /// Get the Environment Argument Value (defaults to dev)
        /// </summary>
        public string Environment
        {
            get
            {
                return _environmentArg.HasValue() ? _environmentArg.Value() : "dev";
            }
        }

        /// <summary>
        /// Get the Project Argument Value (defaults to project)
        /// </summary>
        public string Project
        {
            get
            {
                return _projectArg.HasValue() ? _projectArg.Value() : "project";
            }
        }

        /// <summary>
        /// Get the Stack Id Value (defaults to the values in Environment & Value with format of environment-project)
        /// </summary>
        public string Id
        {
            get
            {
                return _idArg.HasValue() ? _idArg.Value() : $"{Environment}-{Project}";
            }
        }

        /// <summary>
        /// A list of files that should be used in the User Data section
        /// </summary>
        public List<string> UserDataFiles
        {
            get
            {
                return _userDataArgs.HasValue() ? _userDataArgs.Values : new List<string>();
            }
        }

        /// <summary>
        /// A list of Stacks to deploy
        /// </summary>
        public List<string> Stacks
        {
            get
            {
                return _stackArgs.HasValue() ? _stackArgs.Values : new List<string>();
            }
        }

        public Configuration.ConfigSettngs ConfigSettngs
        {
            get
            {
                if (_configurationFileArgs.HasValue())
                {
                    _configSettngs = Configuration.ConfigSettngs.Load(_configurationFileArgs.Value());
                }
                else
                {
                    // for testing
                    _configSettngs = Configuration.ConfigSettngs.Load("/Users/eric.wilson/working/projects/geekcafe/aws-cdk-dot-netcore/aws-cdk-devops-lib/configurations/geekcafe-local/dev/config.json");
                }

                return _configSettngs;
            }
        }

        

    }
}
