using PersonApi.V1.Boundary;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;
using System;
using PersonApi.V1.Infrastructure.JWT;

namespace PersonApi.V1.Factories
{
    

    public interface ISnsFactory
    {
        PersonSns Create(Person person, Token token);
    }

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
                EventType = "PersonCreatedEvent",
                Version = "v1",
                SourceDomain = "Person",
                SourceSystem = "PersonAPI",
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Name = token.Name,
                    Email = token.Email
                }
            };
        }
    }

    public class ResponseFactory : IResponseFactory
    {
        private readonly IApiLinkGenerator _apiLinkGenerator;
        public ResponseFactory(IApiLinkGenerator apiLinkGenerator)
        {
            _apiLinkGenerator = apiLinkGenerator;
        }

        public static string FormatDateOfBirth(DateTime dob)
        {
            return dob.ToString("yyyy-MM-dd");
        }

        public PersonResponseObject ToResponse(Person domain)
        {
            if (null == domain) return null;

            return new PersonResponseObject
            {
                Id = domain.Id,
                Title = domain.Title,
                PreferredFirstName = domain.PreferredFirstname,
                PreferredSurname = domain.PreferredSurname,
                FirstName = domain.Firstname,
                MiddleName = domain.MiddleName,
                Surname = domain.Surname,
                Ethnicity = domain.Ethnicity,
                Nationality = domain.Nationality,
                PlaceOfBirth = domain.PlaceOfBirth,
                DateOfBirth = FormatDateOfBirth(domain.DateOfBirth),
                Gender = domain.Gender,
                Identifications = domain.Identifications,
                Languages = domain.Languages,
                CommunicationRequirements = domain.CommunicationRequirements,
                PersonTypes = domain.PersonTypes,
                Links = _apiLinkGenerator?.GenerateLinksForPerson(domain),
                Tenures = domain.Tenures,
                Reason = domain.Reason
            };
        }
    }
}
