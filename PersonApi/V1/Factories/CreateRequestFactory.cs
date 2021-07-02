using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PersonApi.V1.Factories
{
    public static class CreateRequestFactory
    {
        public static PersonDbEntity ToDatabase(this CreatePersonRequestObject createPersonRequestObject)
        {
            return new PersonDbEntity()
            {
                Id = createPersonRequestObject.Id == Guid.Empty ? Guid.NewGuid() : createPersonRequestObject.Id,
                Title = createPersonRequestObject.Title,
                PreferredTitle = createPersonRequestObject.PreferredTitle,
                PreferredMiddleName = createPersonRequestObject.PreferredMiddleName,
                PreferredFirstName = createPersonRequestObject.PreferredFirstName,
                PreferredSurname = createPersonRequestObject.PreferredSurname,
                FirstName = createPersonRequestObject.FirstName,
                MiddleName = createPersonRequestObject.MiddleName,
                Surname = createPersonRequestObject.Surname,
                Ethnicity = createPersonRequestObject.Ethnicity,
                Nationality = createPersonRequestObject.Nationality,
                NationalInsuranceNo = createPersonRequestObject.NationalInsuranceNo?.ToUpper(),
                PlaceOfBirth = createPersonRequestObject.PlaceOfBirth,
                DateOfBirth = createPersonRequestObject.DateOfBirth,
                Gender = createPersonRequestObject.Gender,
                Reason = createPersonRequestObject.Reason,
                Identifications = createPersonRequestObject.Identifications == null ? new List<Identification>() : createPersonRequestObject.Identifications.Select(x => x).ToList(),
                Languages = createPersonRequestObject.Languages == null ? new List<Language>() : createPersonRequestObject.Languages.Select(x => x).ToList(),
                CommunicationRequirements = createPersonRequestObject.CommunicationRequirements == null ? new List<CommunicationRequirement>() : createPersonRequestObject.CommunicationRequirements.Select(x => x).ToList(),
                PersonTypes = createPersonRequestObject.PersonTypes == null ? new List<PersonType>() : createPersonRequestObject.PersonTypes.Select(x => x).ToList(),
                Tenures = createPersonRequestObject.Tenures == null ? new List<Tenure>() : createPersonRequestObject.Tenures.Select(x => x).ToList()
            };
        }
    }
}
