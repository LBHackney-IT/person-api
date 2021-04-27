using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.Infrastructure;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.Gateways
{
    [Collection("LogCall collection")]
    public class DynamoDbGatewayTests
    {
        private readonly LogCallAspectFixture _logCallFixture;
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IDynamoDBContext> _dynamoDb;
        private readonly Mock<ILogger<DynamoDbGateway>> _logger;
        private readonly DynamoDbGateway _classUnderTest;

        public DynamoDbGatewayTests(LogCallAspectFixture logCallFixture)
        {
            _logCallFixture = logCallFixture;

            _dynamoDb = new Mock<IDynamoDBContext>();
            _logger = new Mock<ILogger<DynamoDbGateway>>();
            _classUnderTest = new DynamoDbGateway(_dynamoDb.Object, _logger.Object);
        }

        [Fact]
        public async Task GetPersonByIdReturnsNullIfEntityDoesntExist()
        {
            // Act
            var id = Guid.NewGuid();
            var response = await _classUnderTest.GetPersonByIdAsync(id).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.LoadAsync for id {id}", Times.Once());
        }

        [Fact]
        public async Task GetPersonByIdReturnsThePersonIfItExists()
        {
            // Arrange
            var entity = _fixture.Create<Person>();
            var dbEntity = entity.ToDatabase();
            var dbIdUsed = entity.Id;

            _dynamoDb.Setup(x => x.LoadAsync<PersonDbEntity>(dbIdUsed, default))
                     .ReturnsAsync(dbEntity);

            // Act
            var response = await _classUnderTest.GetPersonByIdAsync(entity.Id).ConfigureAwait(false);

            // Assert
            _dynamoDb.Verify(x => x.LoadAsync<PersonDbEntity>(dbIdUsed, default), Times.Once);
            entity.Id.Should().Be(response.Id);
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.LoadAsync for id {entity.Id}", Times.Once());
        }

        [Fact]
        public void GetPersonByIdExceptionThrow()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _dynamoDb.Setup(x => x.LoadAsync<PersonDbEntity>(id, default))
                     .ThrowsAsync(exception);

            // Act
            Func<Task<Person>> func = async () => await _classUnderTest.GetPersonByIdAsync(id).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
            _dynamoDb.Verify(x => x.LoadAsync<PersonDbEntity>(id, default), Times.Once);
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.LoadAsync for id {id}", Times.Once());
        }
    }
}
