using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using PersonApi.V1.Boundary.Request;

namespace PersonApi.Tests.V1.E2ETests.Fixtures
{
    public class PersonFixture : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();

        private readonly IDynamoDBContext _dbContext;
        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;

        public PersonDbEntity Person { get; private set; }

        public PersonRequestObject PersonRequest { get; private set; }

        public Guid PersonId { get; private set; }

        public string InvalidPersonId { get; private set; }

        public PersonFixture(IDynamoDBContext dbContext, IAmazonSimpleNotificationService amazonSimpleNotificationService)
        {
            _dbContext = dbContext;
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (null != Person)
                    _dbContext.DeleteAsync<PersonDbEntity>(Person.Id).GetAwaiter().GetResult();

                _disposed = true;
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

        public void GivenANewPersonIsCreated()
        {
            var personRequest = _fixture.Build<PersonRequestObject>()
                .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
                .Create();

            var snsAttrs = new Dictionary<string, string>();
            snsAttrs.Add("fifo_topic", "true");
            snsAttrs.Add("content_based_deduplication", "true");

            var response = _amazonSimpleNotificationService.CreateTopicAsync(new CreateTopicRequest
            {
                Name = "personcreated",
                Attributes = snsAttrs
            }).Result;

            Environment.SetEnvironmentVariable("PersonTopicArn", response.TopicArn);

            PersonRequest = personRequest;
        }

        public void GivenAnInvalidPersonId()
        {
            InvalidPersonId = "12345667890";
        }
    }
}
