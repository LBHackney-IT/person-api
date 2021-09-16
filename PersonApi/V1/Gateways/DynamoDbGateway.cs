using Amazon.DynamoDBv2.DataModel;
using Hackney.Core.Logging;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Boundary.Request;
using Hackney.Shared.Person;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using PersonApi.V1.Infrastructure.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi.V1.Gateways
{
    public class DynamoDbGateway : IPersonApiGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IEntityUpdater _updater;
        private readonly ILogger<DynamoDbGateway> _logger;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, IEntityUpdater updater, ILogger<DynamoDbGateway> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _updater = updater;
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
        public async Task<Person> PostNewPersonAsync(CreatePersonRequestObject requestObject)
        {
            _logger.LogDebug($"Calling IDynamoDBContext.SaveAsync");
            var personDbEntity = requestObject.ToDatabase();

            personDbEntity.LastModified = DateTime.UtcNow;
            await _dynamoDbContext.SaveAsync(personDbEntity).ConfigureAwait(false);

            return personDbEntity.ToDomain();
        }

        [LogCall]
        public async Task<UpdateEntityResult<PersonDbEntity>> UpdatePersonByIdAsync(UpdatePersonRequestObject requestObject, string requestBody,
                                                                                    PersonQueryObject query, int? ifMatch)
        {
            var existingPerson = await _dynamoDbContext.LoadAsync<PersonDbEntity>(query.Id).ConfigureAwait(false);
            if (existingPerson == null) return null;

            if (ifMatch != existingPerson.VersionNumber)
                throw new VersionNumberConflictException(ifMatch, existingPerson.VersionNumber);

            var result = _updater.UpdateEntity(existingPerson, requestBody, requestObject);
            if (result.NewValues.Any())
            {
                _logger.LogDebug($"Calling IDynamoDBContext.SaveAsync to update id {query.Id}");
                result.UpdatedEntity.LastModified = DateTime.UtcNow;
                await _dynamoDbContext.SaveAsync(result.UpdatedEntity).ConfigureAwait(false);
            }

            return result;
        }
    }
}
