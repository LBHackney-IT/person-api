using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi.V1.Domain.Configuration
{
    public class AwsConfiguration
    {
        public string Region { get; set; }

        public string PersonTopicArn { get; set; }
    }
}
