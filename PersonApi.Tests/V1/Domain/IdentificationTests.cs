using AutoFixture;
using FluentAssertions;
using Force.DeepCloner;
using PersonApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PersonApi.Tests.V1.Domain
{
    public class IdentificationTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Identification _classUnderTest;

        public static IEnumerable<object[]> AllIdTypes()
        {
            foreach (var val in Enum.GetValues(typeof(IdentificationType)))
                yield return new object[] { val };
        }

        public IdentificationTests()
        {
            _classUnderTest = _fixture.Create<Identification>();
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
        [MemberData(nameof(AllIdTypes))]
        public void EqualsTestEquivalentObjectReturnsTrue(IdentificationType idType)
        {
            _classUnderTest.IdentificationType = idType;
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
