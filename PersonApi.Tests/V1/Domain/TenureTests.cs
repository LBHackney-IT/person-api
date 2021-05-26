using System;
using System.Globalization;
using FluentAssertions;
using PersonApi.V1.Domain;
using Xunit;

namespace PersonApi.Tests.V1.Domain
{
    public class TenureTests
    {
        private Tenure _classUnderTest;

        public TenureTests()
        {
            _classUnderTest = new Tenure();
        }

        [Fact]
        public void GivenATenureWhenEndDateIsNullThenIsActiveShouldBeTrue()
        {
            // given + when + then
            _classUnderTest.IsActive.Should().BeFalse();
        }

        [Fact]
        public void GivenATenureWhenEndDateIsGreaterThanCurrentDateThenIsActiveShouldBeTrue()
        {
            // given
            _classUnderTest.EndDate = DateTime.Now.AddDays(1).ToShortDateString();

            // when + then
            _classUnderTest.IsActive.Should().BeFalse();
        }

        [Fact]
        public void GivenATenureWhenEndDateIsLessThanCurrentDateThenIsActiveShouldBeFalse()
        {
            // given
            _classUnderTest.EndDate = DateTime.Now.AddDays(-1).ToShortDateString();

            // when + then
            _classUnderTest.IsActive.Should().BeFalse();
        }
    }
}
