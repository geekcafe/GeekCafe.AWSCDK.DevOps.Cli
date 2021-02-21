using System;
using System.Collections.Generic;
using GeekCafe.AWSCDK.DevOps.Core.Utilities;

namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public interface IConfigSettings: IGlobalSettings
    {
        public string Environment { get; set; } 
        public string Company { get; set; }
        public string Project { get; set; }
        public Vpc Vpc { get; set; } 
        public Rds Rds { get; set; } 
        public AutoScalingGroup Asg { get; set; }
        public LoadBalancer Alb { get; set; }
        public List<Tag> Tags { get; set; }
    }
    public class ConfigSettngs: GlobalSettings, IConfigSettings, IGlobalSettings
    {
        
        public Vpc Vpc { get; set; } = new Vpc();
        public Rds Rds { get; set; } = new Rds();
        public AutoScalingGroup Asg { get; set; } = new AutoScalingGroup();
        public LoadBalancer Alb { get; set; } = new LoadBalancer();
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public ConfigSettngs()
        {
            Asg = new AutoScalingGroup(this);
            Vpc = new Vpc(this);
            Rds = new Rds(this);
            Alb = new LoadBalancer(this);

        }
        



        public static ConfigSettngs Load(string path)
        {
            // load the configuration file
            var json = System.IO.File.ReadAllText(path);
            var config = json.ToModel<ConfigSettngs>();

            return config;
        }

        /// <summary>
        /// Create a json configuration file
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string CreateTemplate(string dir)
        {

            dir = dir?.Trim();
            System.IO.Directory.CreateDirectory(dir);

            var file = System.IO.Path.Join(dir, "config-template.json");                        
            var config = new ConfigSettngs();
            var json = config.ToJson();

            System.IO.File.WriteAllText(file, json);

            return file;

        }

    }
}
