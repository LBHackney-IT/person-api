using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;
using System.Linq;

namespace PersonApi.V1.Factories
{
    public static class EntityFactory
    {
        public static Person ToDomain(this PersonDbEntity databaseEntity)
        {
            return new Person
            {
                Id = databaseEntity.Id,
                Title = databaseEntity.Title,
                PreferredTitle = databaseEntity.PreferredTitle,
                PreferredFirstname = databaseEntity.PreferredFirstname,
                PreferredMiddleName = databaseEntity.PreferredMiddleName,
                PreferredSurname = databaseEntity.PreferredSurname,
                Firstname = databaseEntity.Firstname,
                MiddleName = databaseEntity.MiddleName,
                Surname = databaseEntity.Surname,
                Ethnicity = databaseEntity.Ethnicity,
                Nationality = databaseEntity.Nationality,
                NationalInsuranceNo = databaseEntity.NationalInsuranceNo?.ToUpper(),
                PlaceOfBirth = databaseEntity.PlaceOfBirth,
                DateOfBirth = databaseEntity.DateOfBirth,
                Gender = databaseEntity.Gender,
                Reason = databaseEntity.Reason,
                Identifications = databaseEntity.Identifications,
                Languages = databaseEntity.Languages,
                CommunicationRequirements = databaseEntity.CommunicationRequirements,
                PersonTypes = databaseEntity.PersonTypes,
                Tenures = databaseEntity.Tenures
            };
        }

        public static PersonDbEntity ToDatabase(this Person entity)
        {
            return new PersonDbEntity
            {
                Id = entity.Id,
                Title = entity.Title,
                PreferredTitle = entity.PreferredTitle,
                PreferredFirstname = entity.PreferredFirstname,
                PreferredMiddleName = entity.PreferredMiddleName,
                PreferredSurname = entity.PreferredSurname,
                Firstname = entity.Firstname,
                MiddleName = entity.MiddleName,
                Surname = entity.Surname,
                Ethnicity = entity.Ethnicity,
                Nationality = entity.Nationality,
                NationalInsuranceNo = entity.NationalInsuranceNo?.ToUpper(),
                PlaceOfBirth = entity.PlaceOfBirth,
                DateOfBirth = entity.DateOfBirth,
                Gender = entity.Gender,
                Reason = entity.Reason,
                Identifications = entity.Identifications.ToList(),
                Languages = entity.Languages.ToList(),
                CommunicationRequirements = entity.CommunicationRequirements.ToList(),
                PersonTypes = entity.PersonTypes.ToList(),
                Tenures = entity.Tenures.ToList()
            };
        }
    }
}
