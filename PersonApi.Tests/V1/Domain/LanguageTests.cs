using AutoFixture;
using FluentAssertions;
using Force.DeepCloner;
using PersonApi.V1.Domain;
using System.Linq;
using Xunit;

namespace PersonApi.Tests.V1.Domain
{
    public class LanguageTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Language _classUnderTest;

        public LanguageTests()
        {
            _classUnderTest = _fixture.Create<Language>();
        }

        [Fact]
        public void EqualsTestWrongTypeReturnsFalse()
        {
            _classUnderTest.Equals("some string").Should().BeFalse();
        }

        [Fact]
        public void EqualsTestSameObjectReturnsTrue()
        {
            _classUnderTest.Equals(_classUnderTest).Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EqualsTestEquivalentObjectReturnsTrue(bool isPrimary)
        {
            _classUnderTest.IsPrimary = isPrimary;
            var compare = _classUnderTest.DeepClone();
            _classUnderTest.Equals(compare).Should().BeTrue();
        }

        [Fact]
        public void EqualsTestDifferentObjectReturnsFalse()
        {
            var compare = _fixture.Create<Tenure>();
            _classUnderTest.Equals(compare).Should().BeFalse();
        }

        [Fact]
        public void GetHashCodeTest()
        {
            var propValues = _classUnderTest.GetType()
                                .GetProperties()
                                .Select(x => x.GetValue(_classUnderTest).ToString());
            var expected = string.Join("", propValues).GetHashCode();
            _classUnderTest.GetHashCode().Should().Be(expected);
        }
    }
}
