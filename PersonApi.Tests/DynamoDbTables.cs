using Amazon.DynamoDBv2;
using Hackney.Core.Testing.DynamoDb;
using System.Collections.Generic;

namespace PersonApi.Tests
{
    public static class DynamoDbTables
    {
        public static List<TableDef> Tables => new List<TableDef>
        {
            new TableDef { Name = "Persons", KeyName = "id", KeyType = ScalarAttributeType.S }
        };
    }
}
