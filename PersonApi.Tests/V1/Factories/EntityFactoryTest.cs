using AutoFixture;
using FluentAssertions;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using Xunit;

namespace PersonApi.Tests.V1.Factories
{
    public class EntityFactoryTest
    {
        private readonly Fixture _fixture = new Fixture();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("2034732948")]
        public void CanMapADatabaseEntityToADomainObject(string ni)
        {
            var databaseEntity = _fixture.Build<PersonDbEntity>()
                                         .With(x => x.NationalInsuranceNo, ni)
                                         .Create();
            var entity = databaseEntity.ToDomain();

            databaseEntity.Id.Should().Be(entity.Id);
            databaseEntity.Title.Should().Be(entity.Title);
            databaseEntity.PreferredFirstName.Should().Be(entity.PreferredFirstName);
            databaseEntity.PreferredSurname.Should().Be(entity.PreferredSurname);
            databaseEntity.FirstName.Should().Be(entity.FirstName);
            databaseEntity.MiddleName.Should().Be(entity.MiddleName);
            databaseEntity.Surname.Should().Be(entity.Surname);
            databaseEntity.Ethnicity.Should().Be(entity.Ethnicity);
            databaseEntity.Nationality.Should().Be(entity.Nationality);
            databaseEntity.NationalInsuranceNo.Should().Be(entity.NationalInsuranceNo);
            databaseEntity.PlaceOfBirth.Should().Be(entity.PlaceOfBirth);
            databaseEntity.DateOfBirth.Should().Be(entity.DateOfBirth);
            databaseEntity.Gender.Should().Be(entity.Gender);
            databaseEntity.Identifications.Should().BeEquivalentTo(entity.Identifications);
            databaseEntity.Languages.Should().BeEquivalentTo(entity.Languages);
            databaseEntity.CommunicationRequirements.Should().BeEquivalentTo(entity.CommunicationRequirements);
            databaseEntity.PersonTypes.Should().BeEquivalentTo(entity.PersonTypes);
            databaseEntity.Tenures.Should().BeEquivalentTo(entity.Tenures);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("2034732948")]
        public void CanMapADomainEntityToADatabaseObject(string ni)
        {
            var person = _fixture.Build<Person>()
                                 .With(x => x.NationalInsuranceNo, ni)
                                 .Create();
            var databaseEntity = person.ToDatabase();

            person.Id.Should().Be(databaseEntity.Id);
            person.Title.Should().Be(person.Title);
            person.PreferredFirstName.Should().Be(databaseEntity.PreferredFirstName);
            person.PreferredSurname.Should().Be(databaseEntity.PreferredSurname);
            person.FirstName.Should().Be(databaseEntity.FirstName);
            person.MiddleName.Should().Be(databaseEntity.MiddleName);
            person.Surname.Should().Be(databaseEntity.Surname);
            person.Ethnicity.Should().Be(databaseEntity.Ethnicity);
            person.Nationality.Should().Be(databaseEntity.Nationality);
            person.NationalInsuranceNo.Should().Be(databaseEntity.NationalInsuranceNo);
            person.PlaceOfBirth.Should().Be(databaseEntity.PlaceOfBirth);
            person.DateOfBirth.Should().Be(databaseEntity.DateOfBirth);
            person.Gender.Should().Be(databaseEntity.Gender);
            person.Identifications.Should().BeEquivalentTo(databaseEntity.Identifications);
            person.Languages.Should().BeEquivalentTo(databaseEntity.Languages);
            person.CommunicationRequirements.Should().BeEquivalentTo(databaseEntity.CommunicationRequirements);
            person.PersonTypes.Should().BeEquivalentTo(databaseEntity.PersonTypes);
            person.Tenures.Should().BeEquivalentTo(databaseEntity.Tenures);
        }
    }
}
