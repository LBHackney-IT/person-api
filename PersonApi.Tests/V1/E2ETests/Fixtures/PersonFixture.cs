using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Domain;
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
                                     .With(x => x.NationalInsuranceNo, "AA123456C")
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
                                     .With(x => x.NationalInsuranceNo, "AA123456C")
                                     .With(x => x.Tenures, _fixture.Build<Tenure>()
                                              .With(y => y.StartDate, "")
                                              .With(y => y.EndDate, "")
                                              .CreateMany(2).ToList())
                                    .With(x => x.Languages, Enumerable.Empty<Language>().ToList())
                                    .Create();
                _dbContext.SaveAsync<PersonDbEntity>(person).GetAwaiter().GetResult();
                Person = person;
                PersonId = person.Id;
            }
        }


        public void GivenAPersonIdDoesNotExist()
        {
            if (null == Person)
            {
                var person = _fixture.Build<PersonDbEntity>()
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

        public void GivenANewPersonRequest()
        {
            var personRequest = _fixture.Build<CreatePersonRequestObject>()
                .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
                .With(x => x.NationalInsuranceNo, "NZ223344D")
                .With(x => x.Tenures, _fixture.Build<Tenure>()
                                              .With(y => y.StartDate, "")
                                              .With(y => y.EndDate, "")
                                              .CreateMany(2))
                .With(x => x.Languages, Enumerable.Empty<Language>())
                .Create();
            CreateSnsTopic();

            CreatePersonRequest = personRequest;
        }

        public void GivenAUpdatePersonRequest(PersonDbEntity person)
        {

            var personRequest = ToRequest(person);

            if (personRequest != null)
            {
                personRequest.Firstname = "Update";
                personRequest.Surname = "Updating";
            }

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
                .With(x => x.NationalInsuranceNo, "p;idfjgdfosigj")
                .With(x => x.Tenures, _fixture.Build<Tenure>()
                                              .With(y => y.StartDate, "asdwsad")
                                              .With(y => y.EndDate, "asdsad")
                                              .CreateMany(1))
                .With(x => x.Languages, new[] { new Language() })
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

        private UpdatePersonRequestObject ToRequest(PersonDbEntity entity)
        {
            if (entity == null) return null;

            return new UpdatePersonRequestObject
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
                Identifications = entity.Identifications.ToList(),
                Languages = entity.Languages.ToList(),
                CommunicationRequirements = entity.CommunicationRequirements.ToList(),
                PersonTypes = entity.PersonTypes.ToList(),
                Tenures = entity.Tenures.ToList()
            };
        }
    }
}
