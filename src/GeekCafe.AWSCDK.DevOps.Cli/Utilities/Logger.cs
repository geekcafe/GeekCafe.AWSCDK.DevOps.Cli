using System;
using Microsoft.Extensions.Logging;

namespace GeekCafe.AWSCDK.DevOps.Cli.Utilities
{
    public class Logger
    {

        public static void Log(string message, LogLevel level = LogLevel.Debug)
        {
            Core.Logging.Logger.Log(message, level);
        }
    }
}
