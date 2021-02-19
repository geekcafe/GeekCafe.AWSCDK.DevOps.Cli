using System;
using Microsoft.Extensions.Logging;

namespace GeekCafe.AWSCDK.DevOps.Cli.Utils
{
    public class Logger
    {

        public static void Log(string message, LogLevel level = LogLevel.Debug)
        {
            Console.WriteLine($"[{level}][{DateTime.UtcNow.ToString()}]: {message}");
        }
    }
}
