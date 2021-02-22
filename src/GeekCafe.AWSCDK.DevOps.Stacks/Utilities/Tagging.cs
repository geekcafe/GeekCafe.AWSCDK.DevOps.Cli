using System;
using System.Collections.Generic;
using Amazon.CDK;
using GeekCafe.AWSCDK.DevOps.Configuration;

namespace GeekCafe.AWSCDK.DevOps.Stacks.Utilities
{
    public class Tagging
    {
        public static void Tag(IConstruct construct, IConfigSettings config, List<Configuration.Tag> tags)
        {
            foreach (var tag in tags)
            {
                var value = (config != null) ? config.Parse(tag.Value) : tag.Value;
                Amazon.CDK.Tags.Of(construct).Add($"{tag.Name}", $"{value}");
            }
           
        }
    }
}
