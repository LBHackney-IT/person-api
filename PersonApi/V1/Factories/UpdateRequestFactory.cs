using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Infrastructure;
using System.Collections.Generic;
using System.Linq;

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
                Identifications = GetListOrNull(updatePersonRequestObject.Identifications),
                Languages = GetListOrNull(updatePersonRequestObject.Languages),
                CommunicationRequirements = GetListOrNull(updatePersonRequestObject.CommunicationRequirements),
                Tenures = GetListOrNull(updatePersonRequestObject.Tenures)
            };
        }

        private static List<T> GetListOrNull<T>(IEnumerable<T> enumerable)
        {
            return enumerable == null ? null : enumerable.ToList();
        }
    }
}
