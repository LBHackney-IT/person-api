using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FluentAssertions;
using Moq;
using PersonApi.V1;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.Infrastructure;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.Gateways
{
    public class DynamoDbGatewayTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IDynamoDBContext> _dynamoDb;
        private readonly Mock<IApiLogger> _mockLogger;
        private readonly DynamoDbGateway _classUnderTest;

        public DynamoDbGatewayTests()
        {
            _dynamoDb = new Mock<IDynamoDBContext>();
            _mockLogger = new Mock<IApiLogger>();
            _classUnderTest = new DynamoDbGateway(_dynamoDb.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetPersonByIdReturnsNullIfEntityDoesntExist()
        {
            // Act
            var response = await _classUnderTest.GetPersonByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
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
        }
    }
}
