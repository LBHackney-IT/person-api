using Amazon.DynamoDBv2.DocumentModel;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using PersonApi.V1.Infrastructure;
using System;

namespace PersonApi.Tests.V1.Infrastructure
{
    [TestFixture]
    public class DynamoDbObjectConverterTests
    {
        private readonly Fixture _fixture = new Fixture();
        private DynamoDbObjectConverter<SomeObject> _sut;

        [SetUp]
        public void TestSetup()
        {
            _sut = new DynamoDbObjectConverter<SomeObject>();
        }

        [Test]
        public void ToEntryTestNullValueReturnsNull()
        {
            _sut.ToEntry(null).Should().BeEquivalentTo(new DynamoDBNull());
        }

        [Test]
        public void ToEntryTestEnumValueReturnsConvertedValue()
        {
            var obj = _fixture.Create<SomeObject>();
            _sut.ToEntry(obj).Should().BeEquivalentTo(
                Document.FromJson(JsonConvert.SerializeObject(obj, new StringEnumConverter())));
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
        public void FromEntryTestEnumValueReturnsConvertedValue()
        {
            var obj = _fixture.Create<SomeObject>();
            DynamoDBEntry dbEntry = Document.FromJson(
                JsonConvert.SerializeObject(obj, new StringEnumConverter()));

            ((SomeObject) _sut.FromEntry(dbEntry)).Should().BeEquivalentTo(obj);
        }

        [Test]
        public void FromEntryTestInvalidInputNotAnObjectTypeThrows()
        {
            DynamoDBEntry dbEntry = new Primitive { Value = "This is an error" };

            _sut.Invoking((c) => c.FromEntry(dbEntry))
                .Should().Throw<ArgumentException>();
        }

        [Test]
        public void FromEntryTestInvalidInputWrongObjectReturnsEmptyObject()
        {
            var obj = _fixture.Create<SomeOtherObject>();
            DynamoDBEntry dbEntry = Document.FromJson(
                JsonConvert.SerializeObject(obj, new StringEnumConverter()));

            _sut.FromEntry(dbEntry).Should().BeEquivalentTo(new SomeObject());
        }
    }

    public class SomeObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public bool Bool { get; set; }
    }

    public class SomeOtherObject
    {
        public Guid DocId { get; set; }
        public string Description { get; set; }
        public bool SomeBool { get; set; }
    }
}
