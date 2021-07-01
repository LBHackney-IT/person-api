using AutoFixture;
using FluentAssertions;
using PersonApi.V1.Boundary.Request;
using System;
using Xunit;

namespace PersonApi.Tests.V1.Boundary
{
    public class UpdatePersonRequestObjectTests
    {
        [Fact]
        public void ToDatabaseTestEmptyGuidCreatesNewGuid()
        {
            var result = (new UpdatePersonRequestObject()).ToDatabase();
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void ToDatabaseTestSubObjectsAreEqual()
        {
            var request = new Fixture().Create<UpdatePersonRequestObject>();
            var result = request.ToDatabase();
            result.Identifications.Should().BeEquivalentTo(request.Identifications);
            result.Languages.Should().BeEquivalentTo(request.Languages);
            result.CommunicationRequirements.Should().BeEquivalentTo(request.CommunicationRequirements);
            result.PersonTypes.Should().BeEquivalentTo(request.PersonTypes);
            result.Tenures.Should().BeEquivalentTo(request.Tenures);
        }
    }
}
