using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FluentAssertions;
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
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.Gateways
{
    [Collection("Aws collection")]
    public class DynamoDbGatewayTests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ILogger<DynamoDbGateway>> _logger;
        private DynamoDbGateway _classUnderTest;

        private readonly IDynamoDBContext _dynamoDb;

        private readonly List<Action> _cleanup = new List<Action>();

        public DynamoDbGatewayTests(AwsIntegrationTests<Startup> dbTestFixture)
        {
            _dynamoDb = dbTestFixture.DynamoDbContext;
            _logger = new Mock<ILogger<DynamoDbGateway>>();
            _classUnderTest = new DynamoDbGateway(_dynamoDb, _logger.Object);
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

        private PersonQueryObject ConstructQuery(Guid id)
        {
            return new PersonQueryObject() { Id = id };
        }

        private UpdatePersonRequestObject ConstructRequest()
        {
            return new UpdatePersonRequestObject()
            {
                Surname = "Update"
            };
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
        public void GetPersonByIdExceptionThrow()
        {
            // Arrange
            var mockDynamoDb = new Mock<IDynamoDBContext>();
            _classUnderTest = new DynamoDbGateway(mockDynamoDb.Object, _logger.Object);
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

        [Fact]
        public async Task UpdatePersonSuccessfulUpdates()
        {
            // Arrange
            var constructPerson = ConstructCreatePerson();
            var query = ConstructQuery(constructPerson.Id);
            await _dynamoDb.SaveAsync(constructPerson.ToDatabase()).ConfigureAwait(false);
            var constructRequest = ConstructRequest();
            //Act
            await _classUnderTest.UpdatePersonByIdAsync(constructRequest, query).ConfigureAwait(false);

            //Assert
            var load = await _dynamoDb.LoadAsync<PersonDbEntity>(constructPerson.Id).ConfigureAwait(false);
            load.Surname.Should().Be(constructRequest.Surname);
            load.FirstName.Should().Be(constructPerson.FirstName);
            load.PersonTypes.Should().BeEquivalentTo(constructPerson.PersonTypes);
            load.CommunicationRequirements.Should().BeEquivalentTo(constructPerson.CommunicationRequirements);
            load.DateOfBirth.Should().Be(constructPerson.DateOfBirth);
            load.Ethnicity.Should().Be(constructPerson.Ethnicity);
            load.Gender.Should().Be(constructPerson.Gender);
            load.Id.Should().Be(constructPerson.Id);
            load.Identifications.Should().BeEquivalentTo(constructPerson.Identifications);
            load.Languages.Should().BeEquivalentTo(constructPerson.Languages);
            load.MiddleName.Should().Be(constructPerson.MiddleName);
            load.NationalInsuranceNo.Should().Be(constructPerson.NationalInsuranceNo);
            load.Nationality.Should().Be(constructPerson.Nationality);
            load.PlaceOfBirth.Should().Be(constructPerson.PlaceOfBirth);
            load.PreferredFirstName.Should().Be(constructPerson.PreferredFirstName);
            load.PreferredMiddleName.Should().Be(constructPerson.PreferredMiddleName);
            load.PreferredSurname.Should().Be(constructPerson.PreferredSurname);
            load.PreferredTitle.Should().Be(constructPerson.PreferredTitle);
            load.Tenures.Should().BeEquivalentTo(constructPerson.Tenures);
            load.Title.Should().Be(constructPerson.Title);
        }


        [Fact]
        public async Task UpdatePersonByIdReturnsNullIfEntityDoesntExist()
        {
            // Act
            var id = Guid.NewGuid();
            var query = ConstructQuery(id);
            var constructRequest = ConstructRequest();

            var response = await _classUnderTest.UpdatePersonByIdAsync(constructRequest, query).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.SaveAsync to update id {query.Id}", Times.Once());
        }

        [Fact]
        public void UpdatePersonByIdExceptionThrow()
        {
            // Arrange
            var mockDynamoDb = new Mock<IDynamoDBContext>();
            _classUnderTest = new DynamoDbGateway(mockDynamoDb.Object, _logger.Object);
            var id = Guid.NewGuid();
            var query = ConstructQuery(id);
            var constructRequest = ConstructRequest();
            var exception = new ApplicationException("Test exception");
            mockDynamoDb.Setup(x => x.LoadAsync<PersonDbEntity>(id, default))
                        .ThrowsAsync(exception);

            // Act
            Func<Task<UpdatePersonGatewayResult>> func = async () => await _classUnderTest.UpdatePersonByIdAsync(constructRequest, query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
            mockDynamoDb.Verify(x => x.LoadAsync<PersonDbEntity>(id, default), Times.Once);
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.SaveAsync to update id {query.Id}", Times.Once());
        }
    }
}
