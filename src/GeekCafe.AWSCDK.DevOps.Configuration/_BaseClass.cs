using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public abstract class _BaseClass
    {

        internal IGlobalSettings GlobalSettings;
        private string _stackName = "";
        private string _name = "";
        public _BaseClass() { }

        public _BaseClass(IGlobalSettings globalSettings)
        {
            GlobalSettings = globalSettings;
        }

        [JsonIgnore]
        public string Project => GlobalSettings?.Project;
        [JsonIgnore]
        public string Company => GlobalSettings?.Company;
        [JsonIgnore]
        public string Environment => GlobalSettings?.Environment;


        
        public string Name
        {
            get { return $"{Environment}-{Project}-{_name}"; }
            set { _name = value; }
        }

        /// <summary>
        /// Tags
        /// </summary>
        public List<Tag> Tags { get; set; } = new List<Tag>();


        /// <summary>
        /// Name of the CloudFormation Stack which is managed by the CDK
        /// </summary>
        public string StackName
        {
            get { return GetRunTimeStackName(); }
            set { _stackName = value; }
        }
        

        public string GetRunTimeStackName()
        {
            if(GlobalSettings != null)
            {
                return $"{Environment}-{_stackName}";
            }
            else
            {
                return _stackName;
            }
            
        }
    }
}
