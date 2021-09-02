using AutoFixture;
using FluentAssertions;
using Moq;
using PersonApi.V1.Boundary;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PersonApi.Tests.V1.Factories
{
    public class ResponseFactoryTest
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IApiLinkGenerator> _mockLinkGenerator;
        private readonly ResponseFactory _sut;

        private readonly Random _random = new Random();

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

        [Fact]
        public void NullPersonToResponseReturnsNull()
        {
            var response = _sut.ToResponse((Person) null);
            response.Should().BeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanMapADomainPersonToAResponsePerson(bool hasDob)
        {
            DateTime? dob = hasDob ? DateTime.UtcNow.AddYears(-30) : default;
            var person = _fixture.Build<Person>()
                                 .With(x => x.DateOfBirth, dob)
                                 .Create();
            var response = _sut.ToResponse(person);

            response.Id.Should().Be(person.Id);
            response.Title.Should().Be(person.Title);
            response.PreferredFirstName.Should().Be(person.PreferredFirstName);
            response.PreferredSurname.Should().Be(person.PreferredSurname);
            response.FirstName.Should().Be(person.FirstName);
            response.MiddleName.Should().Be(person.MiddleName);
            response.Surname.Should().Be(person.Surname);
            response.PlaceOfBirth.Should().Be(person.PlaceOfBirth);
            response.DateOfBirth.Should().Be(ResponseFactory.FormatDateOfBirth(person.DateOfBirth));
            response.PersonTypes.Should().BeEquivalentTo(person.PersonTypes);
            response.Tenures.Should().BeEquivalentTo(person.Tenures?.Select(x => ResponseFactory.ToResponse(x)));
        }


        // Test that tenures are ordered
        // Active first - newest to oldest
        // Inactive - newest to oldest

        // create list of active tenures - random dates
        // create list of inactive tenures - random dates

        [Fact]
        public void PersonResponseObjectToResponseWhenCalledReturnsTenuresInCorrectOrder()
        {
            var numberOfActiveTenures = _random.Next(2, 5);
            var numberOfInactiveTenures = _random.Next(2, 5);

            // create random list of active and inactive tenures
            var shuffledTenures = CreateListOfShuffledTenures(numberOfActiveTenures, numberOfInactiveTenures);

            // mock person
            var mockPerson = _fixture.Build<Person>().With(x => x.Tenures, shuffledTenures).Create();

            // call method
            var response = _sut.ToResponse(mockPerson);

            // seperate response into what should be active and inactive tenures
            var responseActiveTenures = response.Tenures.Take(numberOfActiveTenures);
            var responseInactiveTenures = response.Tenures.Skip(numberOfActiveTenures).Take(numberOfInactiveTenures);


            // assert first half of tenures are active
            responseActiveTenures.Should().OnlyContain(x => x.IsActive == true);

            // assert second half of tenures are inactive
            responseInactiveTenures.Should().OnlyContain(x => x.IsActive == false);

            // assert first half of tenures is in date order
            responseActiveTenures.Select(x => DateTime.Parse(x.StartDate)).Should().BeInDescendingOrder();

            // assert second half of tenures is in date order
            responseInactiveTenures.Select(x => DateTime.Parse(x.StartDate)).Should().BeInDescendingOrder();
        }

        private List<Tenure> CreateListOfShuffledTenures(int numberOfActiveTenures, int numberOfInactiveTenures)
        {
            // Tenure.IsActive is readonly. Must set enddate to null or date in past
            string activeTenureDateValue = null;
            string inactiveTeunureDateValue = DateTime.UtcNow.AddDays(-7).ToString(); // generate date in past

            // create list of active tenures - random dates
            var activeTenures = _fixture.Build<Tenure>()
                .With(x => x.EndDate, activeTenureDateValue)
                .With(x => x.StartDate, CreateRandomStartDateValue)
                .CreateMany(numberOfActiveTenures);

            // create list of inactive tenures - random dates
            var inactiveTenures = _fixture.Build<Tenure>()
                .With(x => x.EndDate, inactiveTeunureDateValue)
                .With(x => x.StartDate, CreateRandomStartDateValue)
                .CreateMany(numberOfInactiveTenures);

            // combine shuffle list
            var shuffledTenures = activeTenures.Concat(inactiveTenures).OrderBy(item => _random.Next());

            return shuffledTenures.ToList();
        }

        private string CreateRandomStartDateValue()
        {
            // An inactive tenure must have enddate set in past
            var numberOfDaysInPast = _random.Next(-1000, -1);

            return DateTime.UtcNow.AddDays(numberOfDaysInPast).ToString();
        }
    }
}
