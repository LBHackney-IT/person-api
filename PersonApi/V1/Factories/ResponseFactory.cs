using PersonApi.V1.Boundary;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;
using System;
using System.Linq;

namespace PersonApi.V1.Factories
{
    public class ResponseFactory : IResponseFactory
    {
        private readonly IApiLinkGenerator _apiLinkGenerator;
        public ResponseFactory(IApiLinkGenerator apiLinkGenerator)
        {
            _apiLinkGenerator = apiLinkGenerator;
        }

        public static string FormatDateOfBirth(DateTime? dob)
        {
            return dob?.ToString("yyyy-MM-dd");
        }

        public PersonResponseObject ToResponse(Person domain)
        {
            if (null == domain) return null;

            return new PersonResponseObject
            {
                Id = domain.Id,
                Title = domain.Title,
                PreferredTitle = domain.PreferredTitle,
                PreferredFirstName = domain.PreferredFirstName,
                PreferredMiddleName = domain.PreferredMiddleName,
                PreferredSurname = domain.PreferredSurname,
                FirstName = domain.FirstName,
                MiddleName = domain.MiddleName,
                Surname = domain.Surname,
                PlaceOfBirth = domain.PlaceOfBirth,
                DateOfBirth = FormatDateOfBirth(domain.DateOfBirth),
                PersonTypes = domain.PersonTypes,
                Links = _apiLinkGenerator?.GenerateLinksForPerson(domain),
                Tenures = domain.Tenures?.Select(x => ToResponse(x)).ToList(),
                Reason = domain.Reason
            };
        }

        public static TenureResponseObject ToResponse(Tenure tenure)
        {
            return new TenureResponseObject()
            {
                AssetFullAddress = tenure.AssetFullAddress,
                AssetId = tenure.AssetId,
                EndDate = tenure.EndDate,
                Id = tenure.Id,
                IsActive = tenure.IsActive,
                PaymentReference = tenure.PaymentReference,
                PropertyReference = tenure.PropertyReference,
                StartDate = tenure.StartDate,
                Type = tenure.Type,
                Uprn = tenure.Uprn
            };
        }
    }
}
