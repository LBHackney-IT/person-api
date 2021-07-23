using AutoFixture;
using FluentAssertions;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Factories;
using Xunit;

namespace PersonApi.Tests.V1.Boundary
{
    public class UpdatePersonRequestObjectTests
    {
        [Fact]
        public void ToDatabaseTestNullSubObjectsCreatesNull()
        {
            var result = (new UpdatePersonRequestObject()).ToDatabase();
            result.Tenures.Should().BeNull();
        }

        [Fact]
        public void ToDatabaseTestSubObjectsAreEqual()
        {
            var request = new Fixture().Create<UpdatePersonRequestObject>();
            var result = request.ToDatabase();
            result.Tenures.Should().BeEquivalentTo(request.Tenures);
        }
    }
}
