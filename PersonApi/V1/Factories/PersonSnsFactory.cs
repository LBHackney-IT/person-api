using Hackney.Core.JWT;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;
using System;

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
                    Name = token.Name,
                    Email = token.Email
                },
                EventData = new EventData
                {
                    NewData = person
                }
            };
        }

        public PersonSns Update(UpdateEntityResult<PersonDbEntity> updateResult, Token token)
        {
            return new PersonSns
            {
                CorrelationId = Guid.NewGuid(),
                DateTime = DateTime.UtcNow,
                EntityId = updateResult.UpdatedEntity.Id,
                Id = Guid.NewGuid(),
                EventType = UpdatePersonConstants.EVENTTYPE,
                Version = UpdatePersonConstants.V1VERSION,
                SourceDomain = UpdatePersonConstants.SOURCEDOMAIN,
                SourceSystem = UpdatePersonConstants.SOURCESYSTEM,
                User = new User
                {
                    Name = token.Name,
                    Email = token.Email
                },
                EventData = new EventData
                {
                    OldData = updateResult.OldValues,
                    NewData = updateResult.NewValues
                }
            };
        }
    }
}
