using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using PersonApi.Tests.V1.Helper;
using PersonApi.V1.Domain;
using PersonApi.V1.Gateways;
using PersonApi.V1.Infrastructure;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PersonApi.V1.Factories;

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
        public void GetEntityByIdReturnsNullIfEntityDoesntExist()
        {
            var response = _classUnderTest.GetEntityById("123");

            response.Should().BeNull();
        }

        [Test]
        public void GetEntityByIdReturnsTheEntityIfItExists()
        {
            var entity = _fixture.Create<Person>();
            var dbEntity = entity.ToDatabase();

            _dynamoDb.Setup(x => x.LoadAsync<PersonDbEntity>(entity.Id.ToString(), default))
                     .ReturnsAsync(dbEntity);

            var response = _classUnderTest.GetEntityById(entity.Id.ToString());

            _dynamoDb.Verify(x => x.LoadAsync<PersonDbEntity>(entity.Id.ToString(), default), Times.Once);

            entity.Id.Should().Be(response.Id);
        }
    }
}
