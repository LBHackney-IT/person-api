using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using NUnit.Framework;
using System;
using FluentAssertions;
using AutoFixture;
using Moq;
using PersonApi.V1.Boundary;

namespace PersonApi.Tests.V1.Factories
{
    public class ResponseFactoryTest
    {
        private readonly Fixture _fixture = new Fixture();
        private Mock<IApiLinkGenerator> _mockLinkGenerator;
        private ResponseFactory _sut;

        [SetUp]
        public void Setup()
        {
            _mockLinkGenerator = new Mock<IApiLinkGenerator>();
            _sut = new ResponseFactory(_mockLinkGenerator.Object);
        }

        [Test]
        public void FormatDOBTest()
        {
            var dob = DateTime.UtcNow.AddYears(-30);
            var formatted = ResponseFactory.FormatDateOfBirth(dob);
            formatted.Should().Be(dob.ToString("yyyy-MM-dd"));
        }

        [Test]
        public void CanMapADomainPersonToAResponsePerson()
        {
            var person = _fixture.Create<Person>();
            var response = _sut.ToResponse(person);

            response.Id.Should().Be(person.Id);
            response.Title.Should().Be(person.Title);
            response.PreferredFirstname.Should().Be(person.PreferredFirstname);
            response.PreferredSurname.Should().Be(person.PreferredSurname);
            response.Firstname.Should().Be(person.Firstname);
            response.MiddleName.Should().Be(person.MiddleName);
            response.Surname.Should().Be(person.Surname);
            response.Ethinicity.Should().Be(person.Ethinicity);
            response.Nationality.Should().Be(person.Nationality);
            response.PlaceOfBirth.Should().Be(person.PlaceOfBirth);
            response.DateOfBirth.Should().Be(ResponseFactory.FormatDateOfBirth(person.DateOfBirth));
            response.Gender.Should().Be(person.Gender);
            response.Identifications.Should().BeEquivalentTo(person.Identifications);
            response.Languages.Should().BeEquivalentTo(person.Languages);
            response.CommunicationRequirements.Should().BeEquivalentTo(person.CommunicationRequirements);
            response.PersonTypes.Should().BeEquivalentTo(person.PersonTypes);
        }
    }
}
