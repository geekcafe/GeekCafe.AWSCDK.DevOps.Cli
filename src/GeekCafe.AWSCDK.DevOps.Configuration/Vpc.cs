using System;
namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public class Vpc: _BaseClass
    {
        public Vpc() { }
        public Vpc(IGlobalSettings globalSettings) : base(globalSettings) { }
        /// <summary>
        /// The Cidr block
        /// </summary>
        public string Cidr { get; set; }        
        public int NatGateways { get; set; } = 0;
    }
}
