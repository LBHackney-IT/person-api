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
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.Gateways
{
    [Collection("DynamoDb collection")]
    public class DynamoDbGatewayTests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ILogger<DynamoDbGateway>> _logger;
        private DynamoDbGateway _classUnderTest;

        private readonly IDynamoDBContext _dynamoDb;
        private readonly List<Action> _cleanup = new List<Action>();

        public DynamoDbGatewayTests(DynamoDbIntegrationTests<Startup> dbTestFixture)
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
            var entity = _fixture.Build<Person>()
                                 .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
                                 .Create();
            var dbEntity = entity.ToDatabase();

            await _dynamoDb.SaveAsync(dbEntity).ConfigureAwait(false);
            _cleanup.Add(async () => await _dynamoDb.DeleteAsync(dbEntity).ConfigureAwait(false));

            // Act
            var response = await _classUnderTest.GetPersonByIdAsync(entity.Id).ConfigureAwait(false);

            // Assert
            entity.Should().BeEquivalentTo(response);
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.LoadAsync for id {entity.Id}", Times.Once());
        }

        [Fact]
        public void GetPersonByIdExceptionThrow()
        {
            // Arrange
            var mockDynamoDb = new Mock<IDynamoDBContext>();
            _classUnderTest = new DynamoDbGateway(mockDynamoDb.Object, _logger.Object);
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            mockDynamoDb.Setup(x => x.LoadAsync<PersonDbEntity>(id, default))
                        .ThrowsAsync(exception);

            // Act
            Func<Task<Person>> func = async () => await _classUnderTest.GetPersonByIdAsync(id).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
            mockDynamoDb.Verify(x => x.LoadAsync<PersonDbEntity>(id, default), Times.Once);
            _logger.VerifyExact(LogLevel.Debug, $"Calling IDynamoDBContext.LoadAsync for id {id}", Times.Once());
        }
    }
}
