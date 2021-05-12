using PersonApi.V1.Boundary;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;
using System;

namespace PersonApi.V1.Factories
{
    public interface IResponseFactory
    {
        PersonResponseObject ToResponse(Person domain);
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
                Tenures = domain.Tenures
            };
        }
    }
}
