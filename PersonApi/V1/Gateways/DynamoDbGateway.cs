using Amazon.DynamoDBv2.DataModel;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using System.Collections.Generic;

namespace PersonApi.V1.Gateways
{
    public class DynamoDbGateway : IPersonApiGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public List<Person> GetAll()
        {
            return new List<Person>();
        }

        public Person GetEntityById(string id)
        {
            var result = _dynamoDbContext.LoadAsync<PersonDbEntity>(id).GetAwaiter().GetResult();
            return result?.ToDomain();
        }
    }
}
