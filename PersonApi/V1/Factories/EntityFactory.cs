using Hackney.Shared.Person;
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
                PreferredFirstName = databaseEntity.PreferredFirstName,
                PreferredMiddleName = databaseEntity.PreferredMiddleName,
                PreferredSurname = databaseEntity.PreferredSurname,
                FirstName = databaseEntity.FirstName,
                MiddleName = databaseEntity.MiddleName,
                Surname = databaseEntity.Surname,
                PlaceOfBirth = databaseEntity.PlaceOfBirth,
                DateOfBirth = databaseEntity.DateOfBirth,
                Reason = databaseEntity.Reason,
                PersonTypes = databaseEntity.PersonTypes,
                Tenures = databaseEntity.Tenures,
                VersionNumber = databaseEntity.VersionNumber,
                LastModified = databaseEntity.LastModified
            };
        }

        public static PersonDbEntity ToDatabase(this Person entity)
        {
            return new PersonDbEntity
            {
                Id = entity.Id,
                Title = entity.Title,
                PreferredTitle = entity.PreferredTitle,
                PreferredFirstName = entity.PreferredFirstName,
                PreferredMiddleName = entity.PreferredMiddleName,
                PreferredSurname = entity.PreferredSurname,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                Surname = entity.Surname,
                PlaceOfBirth = entity.PlaceOfBirth,
                DateOfBirth = entity.DateOfBirth,
                Reason = entity.Reason,
                PersonTypes = entity.PersonTypes.ToList(),
                Tenures = entity.Tenures.ToList(),
                VersionNumber = entity.VersionNumber,
                LastModified = entity.LastModified
            };
        }
    }
}
