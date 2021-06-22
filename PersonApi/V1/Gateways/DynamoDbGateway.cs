using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal.Transform;
using Hackney.Core.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

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
        public async Task<Person> GetPersonByIdAsync(PersonQueryObject query)
        {
            _logger.LogDebug($"Calling IDynamoDBContext.LoadAsync for id {query.Id}");

            var result = await _dynamoDbContext.LoadAsync<PersonDbEntity>(query.Id).ConfigureAwait(false);
            return result?.ToDomain();
        }

        [LogCall]
        public async Task<Person> PostNewPersonAsync(PersonRequestObject requestObject)
        {
            _logger.LogDebug($"Calling IDynamoDBContext.SaveAsync");
            var personDbEntity = requestObject.ToDatabase();

            await _dynamoDbContext.SaveAsync(personDbEntity).ConfigureAwait(false);

            return personDbEntity.ToDomain();
        }

        [LogCall]
        public async Task<Person> UpdatePersonByIdAsync(PersonRequestObject requestObject)
        {
            _logger.LogDebug($"Calling IDynamoDBContext.SaveAsync to update id {requestObject.Id}");

            var personDbEntity = requestObject.ToDatabase();
            var load = await _dynamoDbContext.LoadAsync(personDbEntity).ConfigureAwait(false);

            if (load == null) return null;

            await _dynamoDbContext.SaveAsync(personDbEntity).ConfigureAwait(false);

            return personDbEntity?.ToDomain();
        }
    }
}
