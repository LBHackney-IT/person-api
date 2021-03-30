using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using PersonApi.V1.Infrastructure;
using System;

namespace PersonApi.Tests.V1.E2ETests.Fixtures
{
    public class PersonFixture : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly IDynamoDBContext _dbContext;
        public PersonDbEntity Person { get; private set; } = null;
        public Guid PersonId { get; private set; }
        public string InvalidPersonId { get; private set; }

        public PersonFixture(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != Person)
                    _dbContext.DeleteAsync<PersonDbEntity>(Person.Id).GetAwaiter().GetResult();
            }
        }

        public void GivenAPersonAlreadyExists()
        {
            if (null == Person)
            {
                var person = _fixture.Build<PersonDbEntity>()
                                     .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
                                     .Create();
                _dbContext.SaveAsync<PersonDbEntity>(person).GetAwaiter().GetResult();
                Person = person;
                PersonId = person.Id;
            }
        }

        public void GivenAPersonDoesNotExist()
        {
            PersonId = Guid.NewGuid();
        }

        public void GivenAnInvalidPersonId()
        {
            InvalidPersonId = "12345667890";
        }
    }
}
