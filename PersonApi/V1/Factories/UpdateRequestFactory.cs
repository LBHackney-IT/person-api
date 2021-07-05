using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PersonApi.V1.Factories
{
    public static class UpdateRequestFactory
    {
        public static PersonDbEntity ToDatabase(this UpdatePersonRequestObject updatePersonRequestObject)
        {
            return new PersonDbEntity()
            {
                Title = updatePersonRequestObject.Title,
                PreferredTitle = updatePersonRequestObject.PreferredTitle,
                PreferredMiddleName = updatePersonRequestObject.PreferredMiddleName,
                PreferredFirstName = updatePersonRequestObject.PreferredFirstName,
                PreferredSurname = updatePersonRequestObject.PreferredSurname,
                FirstName = updatePersonRequestObject.FirstName,
                MiddleName = updatePersonRequestObject.MiddleName,
                Surname = updatePersonRequestObject.Surname,
                Ethnicity = updatePersonRequestObject.Ethnicity,
                Nationality = updatePersonRequestObject.Nationality,
                NationalInsuranceNo = updatePersonRequestObject.NationalInsuranceNo?.ToUpper(),
                PlaceOfBirth = updatePersonRequestObject.PlaceOfBirth,
                DateOfBirth = updatePersonRequestObject.DateOfBirth,
                Gender = updatePersonRequestObject.Gender,
                Identifications = updatePersonRequestObject.Identifications == null ? null : updatePersonRequestObject.Identifications.Select(x => x).ToList(),
                Languages = updatePersonRequestObject.Languages == null ? null : updatePersonRequestObject.Languages.Select(x => x).ToList(),
                CommunicationRequirements = updatePersonRequestObject.CommunicationRequirements == null ? null : updatePersonRequestObject.CommunicationRequirements.Select(x => x).ToList(),
                PersonTypes = updatePersonRequestObject.PersonTypes == null ? null : updatePersonRequestObject.PersonTypes.Select(x => x).ToList(),
                Tenures = updatePersonRequestObject.Tenures == null ? null : updatePersonRequestObject.Tenures.Select(x => x).ToList()
            };
        }
    }
}
