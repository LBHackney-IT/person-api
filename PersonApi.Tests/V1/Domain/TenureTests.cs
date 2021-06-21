using System;
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
            _classUnderTest.IsActive().Should().BeTrue();
        }

        [Fact]
        public void GivenATenureWhenEndDateIsGreaterThanCurrentDateThenIsActiveShouldBeTrue()
        {
            // given
            _classUnderTest.EndDate = DateTime.Now.AddDays(1).ToShortDateString();

            // when + then
            _classUnderTest.IsActive().Should().BeTrue();
        }

        [Fact]
        public void GivenATenureWhenEndDateIsMinimumDateThanCurrentDateThenIsActiveShouldBeTrue()
        {
            // given
            _classUnderTest.EndDate = "1900-01-01";

            // when + then
            _classUnderTest.IsActive().Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GivenATenureWhenEndDateIsLessThanCurrentDateThenIsActiveShouldBeFalse(int daysToAdd)
        {
            // given
            _classUnderTest.EndDate = DateTime.Now.AddDays(daysToAdd).ToShortDateString();

            // when + then
            _classUnderTest.IsActive().Should().BeFalse();
        }
    }
}
