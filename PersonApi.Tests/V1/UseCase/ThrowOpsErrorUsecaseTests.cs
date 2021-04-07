using FluentAssertions;
using PersonApi.V1.UseCase;
using Xunit;

namespace PersonApi.Tests.V1.UseCase
{
    public class ThrowOpsErrorUsecaseTests
    {
        [Fact]
        public void ThrowsTestOpsErrorException()
        {
            var ex = Assert.Throws<TestOpsErrorException>(
                delegate { ThrowOpsErrorUsecase.Execute(); });

            var expected = "This is a test exception to test our integrations";

            ex.Message.Should().BeEquivalentTo(expected);
        }
    }
}
