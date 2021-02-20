using System;
using System.Collections.Generic;

namespace GeekCafe.AWSCDK.DevOps.Configuration
{
    public abstract class _BaseClass
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        /// <summary>
        /// Name of the CloudFormation Stack which is managed by the CDK
        /// </summary>
        public string StackName { get; set; } = "";
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
