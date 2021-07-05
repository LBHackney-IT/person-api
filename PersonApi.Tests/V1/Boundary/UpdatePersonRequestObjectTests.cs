using AutoFixture;
using FluentAssertions;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Factories;
using System;
using Xunit;

namespace PersonApi.Tests.V1.Boundary
{
    public class UpdatePersonRequestObjectTests
    {
        [Fact]
        public void ToDatabaseTestNullSubObjectsCreatesNull()
        {
            var result = (new UpdatePersonRequestObject()).ToDatabase();
            result.Identifications.Should().BeNull();
            result.Languages.Should().BeNull();
            result.CommunicationRequirements.Should().BeNull();
            result.PersonTypes.Should().BeNull();
            result.Tenures.Should().BeNull();
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
