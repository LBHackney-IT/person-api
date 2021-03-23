using AutoFixture;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;

namespace PersonApi.Tests.V1.Helper
{
    public static class DatabaseEntityHelper
    {
        public static PersonDbEntity CreateDatabaseEntity()
        {
            var entity = new Fixture().Create<Person>();

            return CreateDatabaseEntityFrom(entity);
        }

        public static PersonDbEntity CreateDatabaseEntityFrom(Person entity)
        {
            return new PersonDbEntity
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
            };
        }
    }
}
