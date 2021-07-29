using FluentAssertions;
using PersonApi.V1.Domain;
using System;
using Xunit;

namespace PersonApi.Tests.V1.Domain
{
    public class TenureHelpersTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("rtkgjhd;gj")]
        public void IsTenureActiveTestInvalidStringReturnsTrue(string endDate)
        {
            TenureHelpers.IsTenureActive(endDate).Should().BeTrue();
        }

        [Theory]
        [InlineData("1900-01-01")]
        [InlineData("1900-01-01T00:00:00.0000000Z")]
        [InlineData("2052-01-01")]
        public void IsTenureActiveTestActiveStringReturnsTrue(string endDate)
        {
            TenureHelpers.IsTenureActive(endDate).Should().BeTrue();
        }

        [Theory]
        [InlineData("1900-01-02")]
        [InlineData("2012-01-01")]
        [InlineData("2021-03-01T12:12:12.0000000Z")]
        public void IsTenureActiveTestInactiveStringReturnsFalse(string endDate)
        {
            TenureHelpers.IsTenureActive(endDate).Should().BeFalse();
        }

        [Fact]
        public void IsTenureActiveTestNullDateTimeReturnsTrue()
        {
            TenureHelpers.IsTenureActive((DateTime?) null).Should().BeTrue();
        }

        [Theory]
        [InlineData("1900-01-01")]
        [InlineData("1900-01-01T00:00:00.0000000Z")]
        [InlineData("2052-01-01")]
        public void IsTenureActiveTestActiveDateTimeReturnsTrue(string endDate)
        {
            var dt = DateTime.Parse(endDate);
            TenureHelpers.IsTenureActive(dt).Should().BeTrue();
        }

        [Theory]
        [InlineData("1900-01-02")]
        [InlineData("2012-01-01")]
        [InlineData("2021-03-01T12:12:12.0000000Z")]
        public void IsTenureActiveTestInactiveDateTimeReturnsFalse(string endDate)
        {
            var dt = DateTime.Parse(endDate);
            TenureHelpers.IsTenureActive(dt).Should().BeFalse();
        }
    }
}
