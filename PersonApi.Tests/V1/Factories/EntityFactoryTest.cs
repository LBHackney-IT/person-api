using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;

namespace PersonApi.Tests.V1.Factories
{
    [TestFixture]
    public class EntityFactoryTest
    {
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public void CanMapADatabaseEntityToADomainObject()
        {
            var databaseEntity = _fixture.Create<PersonDbEntity>();
            var entity = databaseEntity.ToDomain();

            databaseEntity.Id.Should().Be(entity.Id);
            databaseEntity.Title.Should().Be(entity.Title);
            databaseEntity.PreferredFirstname.Should().Be(entity.PreferredFirstname);
            databaseEntity.PreferredSurname.Should().Be(entity.PreferredSurname);
            databaseEntity.Firstname.Should().Be(entity.Firstname);
            databaseEntity.MiddleName.Should().Be(entity.MiddleName);
            databaseEntity.Surname.Should().Be(entity.Surname);
            databaseEntity.Ethinicity.Should().Be(entity.Ethinicity);
            databaseEntity.Nationality.Should().Be(entity.Nationality);
            databaseEntity.PlaceOfBirth.Should().Be(entity.PlaceOfBirth);
            databaseEntity.DateOfBirth.Should().Be(entity.DateOfBirth);
            databaseEntity.Gender.Should().Be(entity.Gender);
            databaseEntity.Identifications.Should().BeEquivalentTo(entity.Identifications);
            databaseEntity.Languages.Should().BeEquivalentTo(entity.Languages);
            databaseEntity.CommunicationRequirements.Should().BeEquivalentTo(entity.CommunicationRequirements);
            databaseEntity.PersonTypes.Should().BeEquivalentTo(entity.PersonTypes);
        }

        [Test]
        public void CanMapADomainEntityToADatabaseObject()
        {
            var entity = _fixture.Create<Person>();
            var databaseEntity = entity.ToDatabase();

            entity.Id.Should().Be(databaseEntity.Id);
            entity.Title.Should().Be(entity.Title);
            entity.PreferredFirstname.Should().Be(databaseEntity.PreferredFirstname);
            entity.PreferredSurname.Should().Be(databaseEntity.PreferredSurname);
            entity.Firstname.Should().Be(databaseEntity.Firstname);
            entity.MiddleName.Should().Be(databaseEntity.MiddleName);
            entity.Surname.Should().Be(databaseEntity.Surname);
            entity.Ethinicity.Should().Be(databaseEntity.Ethinicity);
            entity.Nationality.Should().Be(databaseEntity.Nationality);
            entity.PlaceOfBirth.Should().Be(databaseEntity.PlaceOfBirth);
            entity.DateOfBirth.Should().Be(databaseEntity.DateOfBirth);
            entity.Gender.Should().Be(databaseEntity.Gender);
            entity.Identifications.Should().BeEquivalentTo(databaseEntity.Identifications);
            entity.Languages.Should().BeEquivalentTo(databaseEntity.Languages);
            entity.CommunicationRequirements.Should().BeEquivalentTo(databaseEntity.CommunicationRequirements);
            entity.PersonTypes.Should().BeEquivalentTo(databaseEntity.PersonTypes);
        }
    }
}
