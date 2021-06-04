using System;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;
using PersonApi.V1.Infrastructure.JWT;

namespace PersonApi.V1.Factories
{
    public class PersonSnsFactory : ISnsFactory
    {
        public PersonSns Create(Person person, Token token)
        {
            return new PersonSns
            {
                CorrelationId = Guid.NewGuid(),
                DateTime = DateTime.UtcNow,
                EntityId = person.Id,
                Id = Guid.NewGuid(),
                EventType = Constants.EVENTTYPE,
                Version = Constants.V1_VERSION,
                SourceDomain = Constants.SOURCE_DOMAIN,
                SourceSystem = Constants.SOURCE_SYSTEM,
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Name = token.Name,
                    Email = token.Email
                }
            };
        }
    }
}
