using AutoFixture;
using PersonApi.Tests.V1.Helper;
using PersonApi.V1.Domain;
using PersonApi.V1.Gateways;
using FluentAssertions;
using NUnit.Framework;

namespace PersonApi.Tests.V1.Gateways
{
    //TODO: Rename Tests to match gateway name
    //For instruction on how to run tests please see the wiki: https://github.com/LBHackney-IT/lbh-base-api/wiki/Running-the-test-suite.
    [TestFixture]
    public class PersonApiGatewayTests : DatabaseTests
    {
        private readonly Fixture _fixture = new Fixture();
        private PersonApiGateway _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new PersonApiGateway(DatabaseContext);
        }
        [Ignore("to do")]
        [Test]
        public void GetEntityByIdReturnsNullIfEntityDoesntExist()
        {
            var response = _classUnderTest.GetEntityById(123);

            response.Should().BeNull();
        }
        [Ignore("to do")]
        [Test]
        public void GetEntityByIdReturnsTheEntityIfItExists()
        {
            var entity = _fixture.Create<Entity>();
            var databaseEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entity);

            DatabaseContext.DatabaseEntities.Add(databaseEntity);
            DatabaseContext.SaveChanges();

            var response = _classUnderTest.GetEntityById(databaseEntity.Id);

            databaseEntity.Id.Should().Be(response.Id);
            databaseEntity.CreatedAt.Should().BeSameDateAs(response.CreatedAt);
        }

        //TODO: Add tests here for the get all method.
    }
}
