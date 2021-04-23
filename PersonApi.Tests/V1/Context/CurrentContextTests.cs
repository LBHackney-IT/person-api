using FluentAssertions;
using PersonApi.V1.Context;
using PersonApi.V1.Controllers;
using System;
using Xunit;

namespace PersonApi.Tests.V1.Context
{
    public class CurrentContextTests
    {
        [Fact]
        public void CurrentContextConstructorTest()
        {
            var sut = new CurrentContext();
            sut.CorrelationId.Should().BeNull();
            sut.UserId.Should().BeNull();
        }

        [Fact]
        public void CurrentContextCorrelationIdTest()
        {
            var sut = new CurrentContext();
            sut.CorrelationId.Should().BeNull();

            var id = Guid.NewGuid();
            sut.CorrelationId = id;

            sut.CorrelationId.Should().Be(id);
            sut.GetValue(Constants.CorrelationId).Should().Be(id);
        }

        [Fact]
        public void CurrentContextUserIdTest()
        {
            var sut = new CurrentContext();
            sut.UserId.Should().BeNull();

            var id = Guid.NewGuid().ToString();
            sut.UserId = id;

            sut.UserId.Should().Be(id);
            sut.GetValue(Constants.UserId).Should().Be(id);
        }


        [Theory]
        [InlineData(10)]
        [InlineData("Some string")]
        [InlineData(true)]
        public void CurrentContextGetValueSetValueTest(object input)
        {
            var sut = new CurrentContext();
            var key = "some-key";

            sut.GetValue(key).Should().BeNull();
            sut.SetValue(key, input);
            sut.GetValue(key).Should().Be(input);
        }
    }
}
