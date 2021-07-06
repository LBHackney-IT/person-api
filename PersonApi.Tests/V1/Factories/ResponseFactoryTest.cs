using AutoFixture;
using FluentAssertions;
using Moq;
using PersonApi.V1.Boundary;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using System;
using Xunit;

namespace PersonApi.Tests.V1.Factories
{
    public class ResponseFactoryTest
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IApiLinkGenerator> _mockLinkGenerator;
        private readonly ResponseFactory _sut;

        public ResponseFactoryTest()
        {
            _mockLinkGenerator = new Mock<IApiLinkGenerator>();
            _sut = new ResponseFactory(_mockLinkGenerator.Object);
        }

        [Fact]
        public void FormatDOBTestReturnsFormattedValue()
        {
            var dob = DateTime.UtcNow.AddYears(-30);
            var formatted = ResponseFactory.FormatDateOfBirth(dob);
            formatted.Should().Be(dob.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public void FormatDOBTestReturnsFormattedNull()
        {
            var formatted = ResponseFactory.FormatDateOfBirth(null);
            formatted.Should().BeNull();
        }

        [Theory]
        [InlineData(true, null)]
        [InlineData(true, "234238945")]
        [InlineData(false, null)]
        [InlineData(false, "234238945")]
        public void CanMapADomainPersonToAResponsePerson(bool hasDob, string ni)
        {
            DateTime? dob = hasDob ? DateTime.UtcNow.AddYears(-30) : default;
            var person = _fixture.Build<Person>()
                                 .With(x => x.DateOfBirth, dob)
                                 .With(x => x.NationalInsuranceNo, ni)
                                 .Create();
            var response = _sut.ToResponse(person);

            response.Id.Should().Be(person.Id);
            response.Title.Should().Be(person.Title);
            response.PreferredFirstName.Should().Be(person.PreferredFirstName);
            response.PreferredSurname.Should().Be(person.PreferredSurname);
            response.FirstName.Should().Be(person.FirstName);
            response.MiddleName.Should().Be(person.MiddleName);
            response.Surname.Should().Be(person.Surname);
            response.Ethnicity.Should().Be(person.Ethnicity);
            response.Nationality.Should().Be(person.Nationality);
            response.PlaceOfBirth.Should().Be(person.PlaceOfBirth);
            response.DateOfBirth.Should().Be(ResponseFactory.FormatDateOfBirth(person.DateOfBirth));
            response.Gender.Should().Be(person.Gender);
            response.Identifications.Should().BeEquivalentTo(person.Identifications);
            response.Languages.Should().BeEquivalentTo(person.Languages);
            response.CommunicationRequirements.Should().BeEquivalentTo(person.CommunicationRequirements);
            response.PersonTypes.Should().BeEquivalentTo(person.PersonTypes);
            response.Tenures.Should().BeEquivalentTo(person.Tenures);
        }
    }
}
