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

        private readonly string _activeTenureDateValue = null;
        private readonly string _inactiveTeunureDateValue = DateTime.UtcNow.AddDays(-7).ToString(); // generate date in past


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

        [Fact]
        public void PersonResponseObjectToResponseWhenCalledOrdersActiveTenuresBeforeInactiveTenures()
        {
            int numberOfActiveTenures = _random.Next(2, 5);
            var numberOfInactiveTenures = _random.Next(2, 5);

            // create many active tenures
            var activeTenures = _fixture.Build<Tenure>().With(x => x.EndDate, _activeTenureDateValue).CreateMany(numberOfActiveTenures);
            // create many inactive tenures
            var inactiveTenures = _fixture.Build<Tenure>().With(x => x.EndDate, _inactiveTeunureDateValue).CreateMany(numberOfInactiveTenures);

            // shuffle list
            var shuffledTenures = ShuffleTenures(activeTenures.Concat(inactiveTenures));

            // create mock person
            var mockPerson = _fixture.Build<Person>().With(x => x.Tenures, shuffledTenures).Create();

            // call method
            var response = _sut.ToResponse(mockPerson);

            var responseActiveTenures = response.Tenures.Take(numberOfActiveTenures);
            var responseInactiveTenures = response.Tenures.Skip(numberOfActiveTenures).Take(numberOfInactiveTenures);

            // assert first half are active
            responseActiveTenures.Should().OnlyContain(x => x.IsActive == true);

            // assert second half are inactive
            responseInactiveTenures.Should().OnlyContain(x => x.IsActive == false);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PersonResponseObjectToResponseWhenCalledOrdersSecureTenuresBeforeOtherTypes(bool activeTenures)
        {
            var tenureEndDate = activeTenures ? _activeTenureDateValue : _inactiveTeunureDateValue;

            int numberOfSecureTenures = _random.Next(2, 5);
            var numberOfOtherTenureTypes = _random.Next(2, 5);

            // create many secure tenures
            var secureTenures = _fixture.Build<Tenure>().With(x => x.EndDate, tenureEndDate).With(x => x.Type, "Secure").CreateMany(numberOfSecureTenures);

            // create many tenures of other types
            var otherTenureTypes = _fixture.Build<Tenure>().With(x => x.EndDate, tenureEndDate).CreateMany(numberOfOtherTenureTypes);

            // shuffle list
            var shuffledTenures = ShuffleTenures(secureTenures.Concat(otherTenureTypes));

            // mock person
            var mockPerson = _fixture.Build<Person>().With(x => x.Tenures, shuffledTenures).Create();

            // call method
            var response = _sut.ToResponse(mockPerson);

            var responseSecureTenures = response.Tenures.Take(numberOfSecureTenures);
            var responseOtherTenureTypes = response.Tenures.Skip(numberOfSecureTenures).Take(numberOfOtherTenureTypes);

            // assert first half are secure
            responseSecureTenures.Should().OnlyContain(x => x.Type == "Secure");

            // assert second half arent secure
            responseOtherTenureTypes.Should().NotContain(x => x.Type == "Secure");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PersonResponseObjectToResponseWhenCalledOrdersTenuresByDate(bool activeTenures)
        {
            var tenureEndDate = activeTenures ? _activeTenureDateValue : _inactiveTeunureDateValue;

            int numberOfSecureTenures = _random.Next(5, 10);
            var numberOfOtherTenureTypes = _random.Next(5, 10);

            // create many secure tenures
            var secureTenures = _fixture.Build<Tenure>()
                .With(x => x.EndDate, tenureEndDate)
                .With(x => x.Type, "Secure")
                .With(x => x.StartDate, CreateRandomStartDateValue)
                .CreateMany(numberOfSecureTenures);

            // create many tenures of other types
            var otherTenureTypes = _fixture
                .Build<Tenure>()
                .With(x => x.EndDate, tenureEndDate)
                .With(x => x.StartDate, CreateRandomStartDateValue)
                .CreateMany(numberOfOtherTenureTypes);

            // shuffle list
            var shuffledTenures = ShuffleTenures(secureTenures.Concat(otherTenureTypes));

            // mock person
            var mockPerson = _fixture.Build<Person>().With(x => x.Tenures, shuffledTenures).Create();

            // call method
            var response = _sut.ToResponse(mockPerson);

            var responseSecureTenures = response.Tenures.Take(numberOfSecureTenures);
            var responseOtherTenureTypes = response.Tenures.Skip(numberOfSecureTenures).Take(numberOfOtherTenureTypes);

            // assert tenures tenures are in date order
            responseSecureTenures.Select(x => DateTime.Parse(x.StartDate)).Should().BeInDescendingOrder();
            responseOtherTenureTypes.Select(x => DateTime.Parse(x.StartDate)).Should().BeInDescendingOrder();
        }

        private IEnumerable<Tenure> ShuffleTenures(IEnumerable<Tenure> list)
        {
            Random random = new Random();

            return list.OrderBy(item => random.Next());
        }

        private string CreateRandomStartDateValue()
        {
            // An inactive tenure must have enddate set in past
            var numberOfDaysInPast = _random.Next(-1000, -1);

            return DateTime.UtcNow.AddDays(numberOfDaysInPast).ToString();
        }
    }
}
