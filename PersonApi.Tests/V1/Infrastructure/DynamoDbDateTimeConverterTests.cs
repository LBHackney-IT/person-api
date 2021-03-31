using Amazon.DynamoDBv2.DocumentModel;
using FluentAssertions;
using NUnit.Framework;
using PersonApi.V1.Infrastructure;
using System;

namespace PersonApi.Tests.V1.Infrastructure
{
    [TestFixture]
    public class DynamoDbDateTimeConverterTests
    {
        private DynamoDbDateTimeConverter _sut;

        [SetUp]
        public void TestSetup()
        {
            _sut = new DynamoDbDateTimeConverter();
        }

        [Test]
        public void ToEntryTestNullValueReturnsNull()
        {
            _sut.ToEntry(null).Should().BeEquivalentTo(new DynamoDBNull());
        }

        [Test]
        public void ToEntryTestDateTimeReturnsConvertedValue()
        {
            DateTime now = DateTime.UtcNow;
            _sut.ToEntry(now).Should().BeEquivalentTo(new Primitive { Value = now.ToString(DynamoDbDateTimeConverter.DATEFORMAT) });
        }

        [Test]
        public void ToEntryTestInvalidInputThrows()
        {
            _sut.Invoking((c) => c.ToEntry("This is an error"))
                .Should().Throw<InvalidCastException>();
        }

        [Test]
        public void FromEntryTestNullValueReturnsNull()
        {
            _sut.FromEntry(null).Should().BeNull();
        }

        [Test]
        public void FromEntryTestDateTimeReturnsConvertedValue()
        {
            DateTime now = DateTime.UtcNow;
            DynamoDBEntry dbEntry = new Primitive { Value = now.ToString(DynamoDbDateTimeConverter.DATEFORMAT) };

            ((DateTime) _sut.FromEntry(dbEntry)).Should().BeCloseTo(now);
        }

        [Test]
        public void FromEntryTestDateOnlyReturnsConvertedValue()
        {
            DateTime now = DateTime.UtcNow;
            DynamoDBEntry dbEntry = new Primitive { Value = now.Date.ToString() };

            ((DateTime) _sut.FromEntry(dbEntry)).Should().BeCloseTo(now.Date);
        }

        [Test]
        public void FromEntryTestInvalidInputThrows()
        {
            DynamoDBEntry dbEntry = new Primitive { Value = "This is an error" };

            _sut.Invoking((c) => c.FromEntry(dbEntry))
                .Should().Throw<FormatException>();
        }
    }
}
