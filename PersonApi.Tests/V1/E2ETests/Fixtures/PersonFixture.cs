using Amazon.DynamoDBv2.DataModel;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using AutoFixture;
using PersonApi.V1.Boundary.Request;
using Hackney.Shared.Person;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PersonApi.Tests.V1.E2ETests.Fixtures
{
    public class PersonFixture : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();

        public readonly IDynamoDBContext _dbContext;
        public readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;

        public PersonDbEntity Person { get; private set; }

        public CreatePersonRequestObject CreatePersonRequest { get; private set; }
        public UpdatePersonRequestObject UpdatePersonRequest { get; private set; }


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
                                     .With(x => x.VersionNumber, (int?) null)
                                    .Create();
                foreach (var tenure in person.Tenures)
                {
                    tenure.EndDate = DateTime.UtcNow.AddDays(1).ToString(CultureInfo.InvariantCulture);
                }
                _dbContext.SaveAsync<PersonDbEntity>(person).GetAwaiter().GetResult();
                Person = person;
                PersonId = person.Id;
            }
        }

        public void GivenAPersonAlreadyExistsAndUpdateRequested()
        {
            if (null == Person)
            {
                var person = _fixture.Build<PersonDbEntity>()
                                     .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
                                     .With(x => x.Title, Title.Mr)
                                     .With(x => x.Tenures, _fixture.Build<Tenure>()
                                              .With(y => y.StartDate, "")
                                              .With(y => y.EndDate, "")
                                              .CreateMany(2).ToList())
                                     .With(x => x.VersionNumber, (int?) null)
                                    .Create();
                _dbContext.SaveAsync<PersonDbEntity>(person).GetAwaiter().GetResult();
                Person = person;
                PersonId = person.Id;
            }
        }


        public void GivenAPersonIdDoesNotExist()
        {
            PersonId = Guid.NewGuid();
        }

        public void GivenAPersonDoesNotExist()
        {
            PersonId = Guid.NewGuid();
        }

        public void GivenANewPersonRequest()
        {
            var personRequest = _fixture.Build<CreatePersonRequestObject>()
                .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
                .With(x => x.Tenures, _fixture.Build<Tenure>()
                                              .With(y => y.StartDate, (string) null)
                                              .With(y => y.EndDate, (string) null)
                                              .CreateMany(2))
                .Create();
            CreateSnsTopic();

            CreatePersonRequest = personRequest;
        }

        public void GivenAUpdatePersonRequest()
        {
            var personRequest = new UpdatePersonRequestObject()
            {
                FirstName = "Update",
                Surname = "Updating",
                Title = Title.Dr
            };

            UpdateSnsTopic();

            UpdatePersonRequest = personRequest;
        }

        public void GivenANewPersonRequestWithValidationErrors()
        {
            var personRequest = _fixture.Build<CreatePersonRequestObject>()
                .With(x => x.FirstName, string.Empty)
                .With(x => x.Surname, string.Empty)
                .With(x => x.PersonTypes, Enumerable.Empty<PersonType>())
                .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(1))
                .With(x => x.Tenures, _fixture.Build<Tenure>()
                                              .With(y => y.StartDate, "asdwsad")
                                              .With(y => y.EndDate, "asdsad")
                                              .CreateMany(1))
                .Create();

            CreateSnsTopic();

            CreatePersonRequest = personRequest;
        }

        public void GivenAnInvalidPersonId()
        {
            InvalidPersonId = "12345667890";
        }

        private void CreateSnsTopic()
        {
            var snsAttrs = new Dictionary<string, string>();
            snsAttrs.Add("fifo_topic", "true");
            snsAttrs.Add("content_based_deduplication", "true");

            var response = _amazonSimpleNotificationService.CreateTopicAsync(new CreateTopicRequest
            {
                Name = "person",
                Attributes = snsAttrs
            }).Result;

            Environment.SetEnvironmentVariable("PERSON_SNS_ARN", response.TopicArn);
        }

        private void UpdateSnsTopic()
        {
            var snsAttrs = new Dictionary<string, string>();
            snsAttrs.Add("fifo_topic", "true");
            snsAttrs.Add("content_based_deduplication", "true");

            var response = _amazonSimpleNotificationService.CreateTopicAsync(new CreateTopicRequest
            {
                Name = "personupdated",
                Attributes = snsAttrs
            }).Result;

            Environment.SetEnvironmentVariable("UPDATED_PERSON_SNS_ARN", response.TopicArn);
        }
    }
}
