using System;
namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public interface IGlobalSettings
    {
        public string Environment { get; set; }
        public string Company { get; set; }
        public string Project { get; set; }
        string Parse(string value);
        string FormatName(string name);
    }

    public class GlobalSettings: IGlobalSettings
    {
        public string Environment { get; set; } = "";
        public string Company { get; set; } = "";
        public string Project { get; set; } = "";


        public string Parse(string value)
        {
            return value.Replace("__ENV__", Environment);
        }

        public string FormatName(string name)
        {
            return $"{Environment}-{Project}-{name}";
        }
    }
}
