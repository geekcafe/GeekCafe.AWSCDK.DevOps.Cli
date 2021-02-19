using System;
namespace GeekCafe.AWSCDK.DevOps.Cli.Commands
{
    public enum ExitCodes
    {
        SUCCESS = 0,
        MISSING_OPTIONS = 1,
        MISSING_CONNECTION_STRING = 2,
        FATEL_ERROR = 3,
    }
}
