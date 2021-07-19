using Amazon.DynamoDBv2.DataModel;
using Hackney.Core.Logging;
using Hackney.Core.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using PersonApi.V1.Infrastructure.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using HeaderConstants = PersonApi.V1.Infrastructure.HeaderConstants;

namespace PersonApi.V1.Gateways
{
    public class DynamoDbGateway : IPersonApiGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IEntityUpdater _updater;
        private readonly ILogger<DynamoDbGateway> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, IEntityUpdater updater, ILogger<DynamoDbGateway> logger, IHttpContextAccessor httpContextAccessor)
        {
            _dynamoDbContext = dynamoDbContext;
            _updater = updater;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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

            await _dynamoDbContext.SaveAsync(personDbEntity).ConfigureAwait(false);

            return personDbEntity.ToDomain();
        }

        [LogCall]
        public async Task<UpdateEntityResult<PersonDbEntity>> UpdatePersonByIdAsync(UpdatePersonRequestObject requestObject, string requestBody, PersonQueryObject query)
        {
            var existingPerson = await _dynamoDbContext.LoadAsync<PersonDbEntity>(query.Id).ConfigureAwait(false);
            if (existingPerson == null) return null;

            // Check the If-Match header value
            string ifMatch = _httpContextAccessor.HttpContext.Request.Headers.GetHeaderValue(HeaderConstants.IfMatch);
            var incomingVersion = int.TryParse(ifMatch, out int i) ? i : (int?) null;

            if (incomingVersion != existingPerson.VersionNumber)
                throw new VersionNumberConflictException(incomingVersion, existingPerson.VersionNumber);

            var result = _updater.UpdateEntity(existingPerson, requestBody, requestObject);
            if (result.NewValues.Any())
            {
                _logger.LogDebug($"Calling IDynamoDBContext.SaveAsync to update id {query.Id}");
                await _dynamoDbContext.SaveAsync(result.UpdatedEntity).ConfigureAwait(false);
            }

            return result;
        }
    }
}
