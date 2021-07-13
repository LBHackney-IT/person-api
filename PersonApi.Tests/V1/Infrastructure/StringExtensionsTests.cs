using FluentAssertions;
using Xunit;

namespace Hackney.Core.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("c", "c")]
        [InlineData("C", "c")]
        [InlineData("camel", "camel")]
        [InlineData("Camel", "camel")]
        [InlineData("oneHumpCamel", "oneHumpCamel")]
        [InlineData("OneHumpCamel", "oneHumpCamel")]
        public void ToCamelCaseTests(string input, string expectedOutput)
        {
            input.ToCamelCase().Should().Be(expectedOutput);
        }
    }
}
