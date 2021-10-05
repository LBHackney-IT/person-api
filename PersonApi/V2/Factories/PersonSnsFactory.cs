using Hackney.Core.JWT;
using Hackney.Shared.Person;
using PersonApi.V1.Domain;
using PersonApi.V2.Infrastructure;
using System;

namespace PersonApi.V2.Factories
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
                Version = CreateEventConstants.V2VERSION,
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
    }
}
