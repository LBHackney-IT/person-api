using Amazon.DynamoDBv2.DataModel;
using System;

namespace PersonApi.V1.Infrastructure
{
    [DynamoDBTable("Persons", LowerCamelCaseProperties = true)]
    public class PersonDbEntity
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }

    }
}
