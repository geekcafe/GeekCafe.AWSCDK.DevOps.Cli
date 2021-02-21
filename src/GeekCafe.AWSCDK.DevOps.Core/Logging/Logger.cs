using System;
using Microsoft.Extensions.Logging;
namespace GeekCafe.AWSCDK.DevOps.Core.Logging
{
    public class Logger
    {
        public static void Log(string message, LogLevel level = LogLevel.Debug)
        {
            Console.WriteLine($"[{level}][{DateTime.UtcNow.ToString()}]: {message}");
        }
    }
}
