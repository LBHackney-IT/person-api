using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.Infrastructure;
using System;
using System.Threading.Tasks;

namespace PersonApi.Tests.V1.Gateways
{
    [TestFixture]
    public class DynamoDbGatewayTests
    {
        private readonly Fixture _fixture = new Fixture();
        private Mock<IDynamoDBContext> _dynamoDb;
        private DynamoDbGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _dynamoDb = new Mock<IDynamoDBContext>();
            _classUnderTest = new DynamoDbGateway(_dynamoDb.Object);
        }

        [Test]
        public async Task GetPersonByIdReturnsNullIfEntityDoesntExist()
        {
            var response = await _classUnderTest.GetPersonByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

            response.Should().BeNull();
        }

        [Test]
        public async Task GetPersonByIdReturnsThePersonIfItExists()
        {
            var entity = _fixture.Create<Person>();
            var dbEntity = entity.ToDatabase();

            _dynamoDb.Setup(x => x.LoadAsync<PersonDbEntity>(entity.Id, default))
                     .ReturnsAsync(dbEntity);

            var response = await _classUnderTest.GetPersonByIdAsync(entity.Id).ConfigureAwait(false);

            _dynamoDb.Verify(x => x.LoadAsync<PersonDbEntity>(entity.Id, default), Times.Once);

            entity.Id.Should().Be(response.Id);
        }

        [Test]
        public void GetPersonByIdExceptionThrow()
        {
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _dynamoDb.Setup(x => x.LoadAsync<PersonDbEntity>(id, default))
                     .ThrowsAsync(exception);

            Func<Task<Person>> func = async () => await _classUnderTest.GetPersonByIdAsync(id).ConfigureAwait(false);

            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
            _dynamoDb.Verify(x => x.LoadAsync<PersonDbEntity>(id, default), Times.Once);
        }
    }
}
