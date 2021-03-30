using PersonApi.V1.Boundary;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;

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

        public PersonResponseObject ToResponse(Person domain)
        {
            return (null == domain)? null :
                new PersonResponseObject
                {
                    Id = domain.Id,
                    Title = domain.Title,
                    PreferredFirstname = domain.PreferredFirstname,
                    PreferredSurname = domain.PreferredSurname,
                    Firstname = domain.Firstname,
                    MiddleName = domain.MiddleName,
                    Surname = domain.Surname,
                    Ethinicity = domain.Ethinicity,
                    Nationality = domain.Nationality,
                    PlaceOfBirth = domain.PlaceOfBirth,
                    DateOfBirth = domain.DateOfBirth,
                    Gender = domain.Gender,
                    Identifications = domain.Identifications,
                    Languages = domain.Languages,
                    CommunicationRequirements = domain.CommunicationRequirements,
                    PersonTypes = domain.PersonTypes,
                    Links = _apiLinkGenerator?.GenerateLinksForPerson(domain)
                };
        }
    }
}
