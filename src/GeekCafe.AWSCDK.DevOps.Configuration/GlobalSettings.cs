using System;
namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public interface IGlobalSettings
    {
        public string Environment { get; set; }
        public string Company { get; set; }
        public string Project { get; set; } 
    }

    public class GlobalSettings: IGlobalSettings
    {
        public string Environment { get; set; } = "";
        public string Company { get; set; } = "";
        public string Project { get; set; } = "";
    }
}
