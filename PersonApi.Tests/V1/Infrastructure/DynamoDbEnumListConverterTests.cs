using Amazon.DynamoDBv2.DocumentModel;
using FluentAssertions;
using NUnit.Framework;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonApi.Tests.V1.Infrastructure
{
    [TestFixture]
    public class DynamoDbEnumListConverterTest
    {
        public enum Number { One, Two, Three, Four, Five }

        private DynamoDbEnumListConverter<Number> _sut;

        [SetUp]
        public void TestSetup()
        {
            _sut = new DynamoDbEnumListConverter<Number>();
        }

        [Test]
        public void ToEntryTestNullValueReturnsNull()
        {
            _sut.ToEntry(null).Should().BeEquivalentTo(new DynamoDBNull());
        }

        [TestCase()]
        [TestCase(Number.One)]
        [TestCase(Number.One, Number.Three)]
        [TestCase(Number.One, Number.Three, Number.Four)]
        public void ToEntryTestEnumValueReturnsConvertedValues(params Number[] args)
        {
            var list = args.ToList();
            _sut.ToEntry(list).Should().BeEquivalentTo(
                new DynamoDBList(list.Select(x => new Primitive(Enum.GetName(typeof(Number), x)))));
        }

        [Test]
        public void ToEntryTestInvalidInputThrows()
        {
            List<string> list = new List<string>(new[] { "Some", "Thing" });
            _sut.Invoking((c) => c.ToEntry(list))
                .Should().Throw<ArgumentException>();
        }

        [Test]
        public void FromEntryTestNullValueReturnsNull()
        {
            _sut.FromEntry(null).Should().BeNull();
        }

        [Test]
        public void FromEntryTestDynamoDBNullReturnsNull()
        {
            _sut.FromEntry(new DynamoDBNull()).Should().BeNull();
        }

        [Test]
        public void FromEntryTestEnumValuesReturnsConvertedValues()
        {
            List<string> list = new List<string>(new[] { "One", "Four", "Five" });
            DynamoDBList dbEntry = DynamoDBList.Create(list);

            List<Number> expected = new List<Number>(new[] { Number.One, Number.Four, Number.Five });
            ((List<Number>) _sut.FromEntry(dbEntry)).Should().BeEquivalentTo(expected);
        }

        [Test]
        public void FromEntryTestInputNotAListThrows()
        {
            DynamoDBEntry dbEntry = new Primitive { Value = "This is an error" };

            _sut.Invoking((c) => c.FromEntry(dbEntry))
                .Should().Throw<ArgumentException>();
        }

        [Test]
        public void FromEntryTestInvalidEnumInputThrows()
        {
            List<string> list = new List<string>(new[] { "One", "Nine", "Five" });
            DynamoDBList dbEntry = DynamoDBList.Create(list);

            _sut.Invoking((c) => c.FromEntry(dbEntry))
                .Should().Throw<ArgumentException>();
        }
    }
}
