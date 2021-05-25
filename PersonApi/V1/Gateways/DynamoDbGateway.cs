using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using PersonApi.V1.Logging;
using System;
using System.Threading.Tasks;
using PersonApi.V1.Boundary.Request;

namespace PersonApi.V1.Gateways
{
    public class DynamoDbGateway : IPersonApiGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger<DynamoDbGateway> _logger;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, ILogger<DynamoDbGateway> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        [LogCall]
        public async Task<Person> GetPersonByIdAsync(Guid id)
        {
            _logger.LogDebug($"Calling IDynamoDBContext.LoadAsync for id {id}");

            var result = await _dynamoDbContext.LoadAsync<PersonDbEntity>(id).ConfigureAwait(false);
            return result?.ToDomain();
        }

        [LogCall]
        public async Task<Person> PostNewPersonAsync(PersonRequestObject requestObject)
        {
            _logger.LogDebug($"Calling IDynamoDBContext.SaveAsync");
            var personDbEntity = requestObject.ToDatabase();

            await _dynamoDbContext.SaveAsync(personDbEntity).ConfigureAwait(false);

            return personDbEntity?.ToDomain();
        }
    }
}
