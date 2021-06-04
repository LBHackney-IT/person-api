using AutoFixture;
using FluentAssertions;
using PersonApi.V1.Boundary.Request;
using System;
using Xunit;

namespace PersonApi.Tests.V1.Boundary
{
    public class PersonRequestObjectTests
    {
        [Fact]
        public void ToDatabaseTestEmptyGuidCreatesNewGuid()
        {
            var result = (new PersonRequestObject()).ToDatabase();
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void ToDatabaseTestNullSubObjectsCreatesDefaults()
        {
            var result = (new PersonRequestObject()).ToDatabase();
            result.Identifications.Should().NotBeNull()
                                       .And.BeEmpty();
            result.Languages.Should().NotBeNull()
                                 .And.BeEmpty();
            result.CommunicationRequirements.Should().NotBeNull()
                                        .And.BeEmpty();
            result.PersonTypes.Should().NotBeNull()
                                   .And.BeEmpty();
            result.Tenures.Should().NotBeNull()
                               .And.BeEmpty();
        }

        [Fact]
        public void ToDatabaseTestSubObjectsAreEqual()
        {
            var request = new Fixture().Create<PersonRequestObject>();
            var result = request.ToDatabase();
            result.Identifications.Should().BeEquivalentTo(request.Identifications);
            result.Languages.Should().BeEquivalentTo(request.Languages);
            result.CommunicationRequirements.Should().BeEquivalentTo(request.CommunicationRequirements);
            result.PersonTypes.Should().BeEquivalentTo(request.PersonTypes);
            result.Tenures.Should().BeEquivalentTo(request.Tenures);
        }
    }
}
