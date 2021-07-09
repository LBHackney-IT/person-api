using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.Extensions.Logging;
using Moq;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.Gateways
{
    [Collection("Aws collection")]
    public class DynamoDbGatewayTests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ILogger<DynamoDbGateway>> _logger;
        private readonly Mock<IEntityUpdater> _mockUpdater;
        private DynamoDbGateway _classUnderTest;

        private readonly IDynamoDBContext _dynamoDb;

        private readonly List<Action> _cleanup = new List<Action>();

        private const string RequestBody = "{ \"firstName\": \"new first name\", \"placeOfBirth\": \"Towcester\" }";

        public DynamoDbGatewayTests(AwsIntegrationTests<Startup> dbTestFixture)
        {
            _dynamoDb = dbTestFixture.DynamoDbContext;
            _mockUpdater = new Mock<IEntityUpdater>();
            _logger = new Mock<ILogger<DynamoDbGateway>>();
            _classUnderTest = new DynamoDbGateway(_dynamoDb, _mockUpdater.Object, _logger.Object);
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
                foreach (var action in _cleanup)
                    action();

                _disposed = true;
            }
        }

        private static JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        private PersonQueryObject ConstructQuery(Guid id)
        {
            return new PersonQueryObject() { Id = id };
        }

        private UpdatePersonRequestObject ConstructRequest()
        {
            return JsonSerializer.Deserialize<UpdatePersonRequestObject>(RequestBody, CreateJsonOptions());
        }

        private CreatePersonRequestObject ConstructCreatePerson(bool nullOptionalEnums = false)

        {
            var person = _fixture.Build<CreatePersonRequestObject>()
                            .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
                            .With(x => x.NationalInsuranceNo, "NZ223344E")
                            .Create();
            if (nullOptionalEnums)
            {
                person.Gender = null;
                person.PreferredTitle = null;
            }

            return person;
        }

        private Person ConstructPerson(bool nullOptionalEnums = false)

        {
            var person = _fixture.Build<Person>()
                            .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
                            .With(x => x.NationalInsuranceNo, "NZ223344E")
                            .Create();
            if (nullOptionalEnums)
            {
                person.Gender = null;
                person.PreferredTitle = null;
            }

            return person;
        }

        [Fact]
        public async Task GetPersonByIdReturnsNullIfEntityDoesntExist()
        {
            // Act
            var id = Guid.NewGuid();
            var query = ConstructQuery(id);
            var response = await _classUnderTest.GetPersonByIdAsync(query).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.LoadAsync for id {id}", Times.Once());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetPersonByIdReturnsThePersonIfItExists(bool nullOptionalEnums)
        {
            // Arrange
            var entity = ConstructCreatePerson(nullOptionalEnums);
            entity.Tenures = new[] { entity.Tenures.First() };
            entity.Tenures.First().EndDate = DateTime.UtcNow.AddYears(-30).ToShortDateString();

            var dbEntity = entity.ToDatabase();

            await _dynamoDb.SaveAsync(dbEntity).ConfigureAwait(false);
            _cleanup.Add(async () => await _dynamoDb.DeleteAsync(dbEntity).ConfigureAwait(false));

            // Act
            var query = ConstructQuery(entity.Id);
            var response = await _classUnderTest.GetPersonByIdAsync(query).ConfigureAwait(false);

            // Assert
            entity.Should().BeEquivalentTo(response);
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.LoadAsync for id {entity.Id}", Times.Once());
        }

        [Fact]
        public void GetPersonByIdExceptionThrow()
        {
            // Arrange
            var mockDynamoDb = new Mock<IDynamoDBContext>();
            _classUnderTest = new DynamoDbGateway(mockDynamoDb.Object, _mockUpdater.Object, _logger.Object);
            var id = Guid.NewGuid();
            var query = ConstructQuery(id);
            var exception = new ApplicationException("Test exception");
            mockDynamoDb.Setup(x => x.LoadAsync<PersonDbEntity>(id, default))
                        .ThrowsAsync(exception);

            // Act
            Func<Task<Person>> func = async () => await _classUnderTest.GetPersonByIdAsync(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
            mockDynamoDb.Verify(x => x.LoadAsync<PersonDbEntity>(id, default), Times.Once);
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.LoadAsync for id {id}", Times.Once());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PostNewPersonSuccessfulSaves(bool nullOptionalEnums)
        {
            // Arrange
            var entity = ConstructCreatePerson(nullOptionalEnums);
            entity.Tenures = new[] { entity.Tenures.First() };
            entity.Tenures.First().EndDate = DateTime.UtcNow.AddYears(-30).ToShortDateString();

            var dbEntity = entity.ToDatabase();

            // Act
            await _dynamoDb.SaveAsync(dbEntity).ConfigureAwait(false);

            // Assert
            var query = ConstructQuery(entity.Id);
            var response = await _classUnderTest.GetPersonByIdAsync(query).ConfigureAwait(false);
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdatePersonSuccessfulUpdates()
        {
            // Arrange
            var person = ConstructPerson();
            var query = ConstructQuery(person.Id);
            await _dynamoDb.SaveAsync(person.ToDatabase()).ConfigureAwait(false);
            var constructRequest = ConstructRequest();

            var updatedPerson = person.DeepClone();
            updatedPerson.FirstName = constructRequest.FirstName;
            updatedPerson.PlaceOfBirth = constructRequest.PlaceOfBirth;
            _mockUpdater.Setup(x => x.UpdateEntity(It.IsAny<PersonDbEntity>(), RequestBody, constructRequest))
                        .Returns(new UpdateEntityResult<PersonDbEntity>()
                        {
                            UpdatedEntity = updatedPerson.ToDatabase(),
                            OldValues = new Dictionary<string, object>
                            {
                                { "firstName", person.FirstName },
                                { "placeOfBirth", person.PlaceOfBirth }
                            },
                            NewValues = new Dictionary<string, object>
                            {
                                { "firstName", updatedPerson.FirstName },
                                { "placeOfBirth", updatedPerson.PlaceOfBirth }
                            }
                        });

            //Act
            await _classUnderTest.UpdatePersonByIdAsync(constructRequest, RequestBody, query).ConfigureAwait(false);

            //Assert
            var load = await _dynamoDb.LoadAsync<PersonDbEntity>(person.Id).ConfigureAwait(false);

            // Changed
            load.FirstName.Should().Be(updatedPerson.FirstName);
            load.PlaceOfBirth.Should().Be(updatedPerson.PlaceOfBirth);

            // Not changed
            load.Surname.Should().Be(person.Surname);
            load.CommunicationRequirements.Should().BeEquivalentTo(person.CommunicationRequirements);
            load.DateOfBirth.Should().Be(person.DateOfBirth);
            load.Ethnicity.Should().Be(person.Ethnicity);
            load.Gender.Should().Be(person.Gender);
            load.Id.Should().Be(person.Id);
            load.Identifications.Should().BeEquivalentTo(person.Identifications);
            load.Languages.Should().BeEquivalentTo(person.Languages);
            load.MiddleName.Should().Be(person.MiddleName);
            load.NationalInsuranceNo.Should().Be(person.NationalInsuranceNo);
            load.Nationality.Should().Be(person.Nationality);
            load.PersonTypes.Should().BeEquivalentTo(person.PersonTypes);
            load.PreferredFirstName.Should().Be(person.PreferredFirstName);
            load.PreferredMiddleName.Should().Be(person.PreferredMiddleName);
            load.PreferredSurname.Should().Be(person.PreferredSurname);
            load.PreferredTitle.Should().Be(person.PreferredTitle);
            load.Tenures.Should().BeEquivalentTo(person.Tenures);
            load.Title.Should().Be(person.Title);
        }

        [Fact]
        public async Task UpdatePersonByIdReturnsNullIfEntityDoesntExist()
        {
            // Act
            var id = Guid.NewGuid();
            var query = ConstructQuery(id);
            var constructRequest = ConstructRequest();

            var response = await _classUnderTest.UpdatePersonByIdAsync(constructRequest, RequestBody, query).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.SaveAsync to update id {query.Id}", Times.Once());
        }

        [Fact]
        public void UpdatePersonByIdExceptionThrow()
        {
            // Arrange
            var mockDynamoDb = new Mock<IDynamoDBContext>();
            _classUnderTest = new DynamoDbGateway(mockDynamoDb.Object, _mockUpdater.Object, _logger.Object);
            var id = Guid.NewGuid();
            var query = ConstructQuery(id);
            var constructRequest = ConstructRequest();
            var exception = new ApplicationException("Test exception");
            mockDynamoDb.Setup(x => x.LoadAsync<PersonDbEntity>(id, default))
                        .ThrowsAsync(exception);

            // Act
            Func<Task<UpdateEntityResult<PersonDbEntity>>> func = async () => await _classUnderTest.UpdatePersonByIdAsync(constructRequest, RequestBody, query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
            mockDynamoDb.Verify(x => x.LoadAsync<PersonDbEntity>(id, default), Times.Once);
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.SaveAsync to update id {query.Id}", Times.Once());
        }
    }
}
