using System;
using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;

namespace GeekCafe.AWSCDK.DevOps.Cli.Commands
{

    public interface ICommandFactoryItem
    {
        /// <summary>
        /// The Name of the command to be registered
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The Command Builder
        /// </summary>
        /// <param name="command">A command that will execute</param>
        /// <returns>CommandLineApplication or rather it's execution point</returns>
        public CommandLineApplication BuildCommand(CommandLineApplication command);

    }

    public class CommandFactory
    {
        /// <summary>
        /// List of Available Commands
        /// </summary>
        public IList<ICommandFactoryItem> Commands { get; } = new List<ICommandFactoryItem>();


        /// <summary>
        /// Build the factory of items
        /// </summary>
        /// <param name="Provider"></param>
        public CommandFactory()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the class and the Commands List
        /// </summary>
        private void Initialize()
        {
            // load all matching commands
            var types = Utilities.FactoryHelpers.GetMatchingTypes(typeof(ICommandFactoryItem));

            foreach (var type in types)
            {
                var command = Activator.CreateInstance(type) as ICommandFactoryItem;

                if (command != null)
                {
                    Commands.Add(command);
                }
            }
        }
    }
}
