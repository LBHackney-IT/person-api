using Amazon.DynamoDBv2.DataModel;
using System;

namespace PersonApi.V1.Infrastructure
{
    [DynamoDBTable("RefGenerator", LowerCamelCaseProperties = true)]
    public class RefGeneratorEntity
    {
        [DynamoDBHashKey]
        public string RefName { get; set; }
        public int RefValue { get; set; }
        public DateTime LastModified { get; set; }

    }
}
