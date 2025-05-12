using Amazon.DynamoDBv2.DataModel;
using Hackney.Core.DynamoDb.Converters;
using Hackney.Shared.Person.Domain;
using System.Collections.Generic;
using System;

namespace PersonApi.V1.Gateways
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
