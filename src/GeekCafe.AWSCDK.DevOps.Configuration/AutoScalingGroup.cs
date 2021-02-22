using System;
namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public class AutoScalingGroup : _BaseClass
    {
        private string _keyName = "";

        public AutoScalingGroup() { }
        public AutoScalingGroup(IGlobalSettings globalSettings): base(globalSettings) { }
        
        /// <summary>
        /// The KeyName (pem key) which must exists in AWS or the stack will fail
        /// </summary>
        public string KeyName {
            get
            {
                if(GlobalSettings != null)
                {
                    return GlobalSettings.Parse(_keyName);
                }
                return _keyName;
            }
            set { _keyName = value; }
        }

        public int? DesiredCapacity { get; set; } = null;
        public int MinCapacity { get; set; } = 1;
        public int MaxCapacity { get; set; } = 1;
        public bool AssociatePublicIpAddress { get; set; } = true;
        public bool? AllowAllOutbound { get; set; } = true;
        /// <summary>
        /// Path to user data directory or a specific file
        /// </summary>
        public string UserDataPath { get; set; }
    }
}
