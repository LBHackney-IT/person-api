using Amazon.DynamoDBv2.DataModel;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using System;
using System.Threading.Tasks;

namespace PersonApi.V1.Gateways
{
    public class DynamoDbGateway : IPersonApiGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<Person> GetPersonByIdAsync(Guid id)
        {
            var result = await _dynamoDbContext.LoadAsync<PersonDbEntity>(EntityFactory.NormaliseDbId(id)).ConfigureAwait(false);
            return result?.ToDomain();
        }
    }
}
