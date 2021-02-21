using System;
namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public class LoadBalancer : _BaseClass
    {
        public LoadBalancer() { }
        public LoadBalancer(IGlobalSettings globalSettings) : base(globalSettings) { }
        public string CertArn { get; set; }
    }
}
