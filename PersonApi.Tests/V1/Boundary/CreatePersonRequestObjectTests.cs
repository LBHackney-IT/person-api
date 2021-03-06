using AutoFixture;
using FluentAssertions;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Factories;
using System;
using Xunit;

namespace PersonApi.Tests.V1.Boundary
{
    public class CreatePersonRequestObjectTests
    {
        [Fact]
        public void ToDatabaseTestEmptyGuidCreatesNewGuid()
        {
            var result = (new CreatePersonRequestObject()).ToDatabase();
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void ToDatabaseTestNullSubObjectsCreatesDefaults()
        {
            var result = (new CreatePersonRequestObject()).ToDatabase();
            result.PersonTypes.Should().NotBeNull()
                                   .And.BeEmpty();
            result.Tenures.Should().NotBeNull()
                               .And.BeEmpty();
        }

        [Fact]
        public void ToDatabaseTestSubObjectsAreEqual()
        {
            var request = new Fixture().Create<CreatePersonRequestObject>();
            var result = request.ToDatabase();
            result.PersonTypes.Should().BeEquivalentTo(request.PersonTypes);
            result.Tenures.Should().BeEquivalentTo(request.Tenures);
        }
    }
}
