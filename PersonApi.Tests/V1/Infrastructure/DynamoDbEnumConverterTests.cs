using Amazon.DynamoDBv2.DocumentModel;
using FluentAssertions;
using NUnit.Framework;
using PersonApi.V1.Infrastructure;
using System;

namespace PersonApi.Tests.V1.Infrastructure
{
    [TestFixture]
    public class DynamoDbEnumConverterTests
    {
        public enum Number { One, Two, Three, Four, Five }

        private DynamoDbEnumConverter<Number> _sut;

        [SetUp]
        public void TestSetup()
        {
            _sut = new DynamoDbEnumConverter<Number>();
        }

        [Test]
        public void ToEntryTestNullValueReturnsNull()
        {
            _sut.ToEntry(null).Should().BeEquivalentTo(new DynamoDBNull());
        }

        [Test]
        public void ToEntryTestEnumValueReturnsConvertedValue()
        {
            var value = Number.Five;
            _sut.ToEntry(value).Should().BeEquivalentTo(new Primitive { Value = "Five" });
        }

        [Test]
        public void ToEntryTestInvalidInputThrows()
        {
            _sut.Invoking((c) => c.ToEntry("This is an error"))
                .Should().Throw<ArgumentException>();
        }

        [Test]
        public void FromEntryTestNullValueReturnsEnumDefault()
        {
            _sut.FromEntry(null).Should().BeEquivalentTo(default(Number));
        }

        [Test]
        public void FromEntryTestDynamoDBNullReturnsEnumDefault()
        {
            _sut.FromEntry(new DynamoDBNull()).Should().BeEquivalentTo(default(Number));
        }

        [Test]
        public void FromEntryTestEnumValueReturnsConvertedValue()
        {
            var stringValue = "Three";
            DynamoDBEntry dbEntry = new Primitive { Value = stringValue };

            ((Number)_sut.FromEntry(dbEntry)).Should().Be(Number.Three);
        }

        [Test]
        public void FromEntryTestInvalidInputThrows()
        {
            DynamoDBEntry dbEntry = new Primitive { Value = "This is an error" };

            _sut.Invoking((c) => c.FromEntry(dbEntry))
                .Should().Throw<ArgumentException>();
        }
    }
}
