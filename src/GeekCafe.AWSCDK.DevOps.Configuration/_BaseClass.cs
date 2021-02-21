using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public abstract class _BaseClass
    {

        private IGlobalSettings _globalSettings;
        private string _stackName = "";
        private string _name = "";
        public _BaseClass() { }

        public _BaseClass(IGlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }

        [JsonIgnore]
        public string Project => _globalSettings?.Project;
        [JsonIgnore]
        public string Company => _globalSettings?.Company;
        [JsonIgnore]
        public string Environment => _globalSettings?.Environment;


        //public string Id { get; set; } = "";
        public string Name
        {
            get
            {
                return $"{Environment}-{Project}-{_name}";
            }
            set
            {
                _name = value;
            }
        }
        /// <summary>
        /// Name of the CloudFormation Stack which is managed by the CDK
        /// </summary>
        public string StackName
        {
            get { return GetRunTimeStackName(); }
            set { _stackName = value; }
        }
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public string GetRunTimeStackName()
        {
            if(_globalSettings != null)
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
