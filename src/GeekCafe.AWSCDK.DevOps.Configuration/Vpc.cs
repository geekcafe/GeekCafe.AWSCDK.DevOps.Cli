using System;
namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public class Vpc: _BaseClass
    {     
        /// <summary>
        /// The Cidr block
        /// </summary>
        public string Cidr { get; set; }        
        public int NatGateways { get; set; } = 0;
    }
}
