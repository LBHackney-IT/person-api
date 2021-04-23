using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using PersonApi.V1.Logging;
using System;
using System.Threading.Tasks;

namespace PersonApi.V1.Gateways
{
    public class DynamoDbGateway : IPersonApiGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IApiLogger _apiLogger;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext,
            IApiLogger apiLogger)
        {
            _dynamoDbContext = dynamoDbContext;
            _apiLogger = apiLogger;
        }

        public async Task<Person> GetPersonByIdAsync(Guid id)
        {
            _apiLogger.Log(LogLevel.Trace, $"Calling DynamoDb LoadAsync for id {id}");

            var result = await _dynamoDbContext.LoadAsync<PersonDbEntity>(id).ConfigureAwait(false);
            return result?.ToDomain();
        }
    }
}
