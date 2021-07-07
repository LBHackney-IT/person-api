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
                EventType = CreateEventConstants.EVENTTYPE,
                Version = CreateEventConstants.V1VERSION,
                SourceDomain = CreateEventConstants.SOURCEDOMAIN,
                SourceSystem = CreateEventConstants.SOURCESYSTEM,
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Name = token.Name,
                    Email = token.Email
                },
                EventData = new EventData
                {
                    NewData = person
                }
            };
        }

        public PersonSns Update(Person old, Person updated, Token token)
        {
            return new PersonSns
            {
                CorrelationId = Guid.NewGuid(),
                DateTime = DateTime.UtcNow,
                EntityId = old.Id,
                Id = Guid.NewGuid(),
                EventType = UpdatePersonConstants.EVENTTYPE,
                Version = UpdatePersonConstants.V1VERSION,
                SourceDomain = UpdatePersonConstants.SOURCEDOMAIN,
                SourceSystem = UpdatePersonConstants.SOURCESYSTEM,
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Name = token.Name,
                    Email = token.Email
                },
                EventData = new EventData
                {
                    OldData = old,
                    NewData = updated
                }
            };
        }
    }
}
