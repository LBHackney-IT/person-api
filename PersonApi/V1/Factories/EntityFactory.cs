using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;

namespace PersonApi.V1.Factories
{
    public static class EntityFactory
    {
        public static Person ToDomain(this PersonDbEntity databaseEntity)
        {
            //TODO: Map the rest of the fields in the domain object.
            // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings

            return new Person
            {
                Id = databaseEntity.Id,
                CreatedAt = databaseEntity.CreatedAt
            };
        }

        public static PersonDbEntity ToDatabase(this Person entity)
        {
            //TODO: Map the rest of the fields in the database object.

            return new PersonDbEntity
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
