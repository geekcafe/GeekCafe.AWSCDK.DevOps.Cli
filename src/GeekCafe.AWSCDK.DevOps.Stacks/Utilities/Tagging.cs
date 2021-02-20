using System;
using System.Collections.Generic;
using Amazon.CDK;
using GeekCafe.AWSCDK.DevOps.Configuration;

namespace GeekCafe.AWSCDK.DevOps.Stacks.Utilities
{
    public class Tagging
    {
        public static void Tag(IConstruct construct, List<Configuration.Tag> tags)
        {
            foreach (var tag in tags)
            {
                Amazon.CDK.Tags.Of(construct).Add($"{tag.Name}", $"{tag.Value}");
            }
           
        }
    }
}
