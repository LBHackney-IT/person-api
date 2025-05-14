using Amazon.DynamoDBv2.DataModel;
using Hackney.Core.DynamoDb.Converters;
using System;

namespace PersonApi.V1.Infrastructure
{
    [DynamoDBTable("RefGenerator", LowerCamelCaseProperties = true)]
    public class RefGeneratorEntity
    {
        [DynamoDBHashKey]
        public string RefName { get; set; }
        public int RefValue { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime LastModified { get; set; }

    }
}
