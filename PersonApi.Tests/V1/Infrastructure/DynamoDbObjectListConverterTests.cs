using Amazon.DynamoDBv2.DocumentModel;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonApi.Tests.V1.Infrastructure
{
    [TestFixture]
    public class DynamoDbObjectListConverterTests
    {
        private readonly Fixture _fixture = new Fixture();
        private DynamoDbObjectListConverter<SomeObject> _sut;

        private List<T> CreateObjectList<T>() where T : class
        {
            return new List<T>(new[]
            {
                _fixture.Create<T>(),
                _fixture.Create<T>(),
                _fixture.Create<T>()
            });
        }

        [SetUp]
        public void TestSetup()
        {
            _sut = new DynamoDbObjectListConverter<SomeObject>();
        }

        [Test]
        public void ToEntryTestNullValueReturnsNull()
        {
            _sut.ToEntry(null).Should().BeEquivalentTo(new DynamoDBNull());
        }

        [Test]
        public void ToEntryTestEnumValueReturnsConvertedObjects()
        {
            var list = CreateObjectList<SomeObject>();
            _sut.ToEntry(list).Should().BeEquivalentTo(
                new DynamoDBList(list.Select(x => Document.FromJson(JsonConvert.SerializeObject(x, new StringEnumConverter())))));
        }

        [Test]
        public void ToEntryTestInvalidInputThrows()
        {
            List<SomeOtherObject> list = new List<SomeOtherObject>(new[] { _fixture.Create<SomeOtherObject>() });
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
        public void FromEntryTestObjectListReturnsConvertedList()
        {
            var list = CreateObjectList<SomeObject>();
            var dbEntry = new DynamoDBList(
                list.Select(x => Document.FromJson(JsonConvert.SerializeObject(x, new StringEnumConverter()))));

            _sut.FromEntry(dbEntry).Should().BeEquivalentTo(list);
        }

        [Test]
        public void FromEntryTestInputNotAListThrows()
        {
            DynamoDBEntry dbEntry = new Primitive { Value = "This is an error" };

            _sut.Invoking((c) => c.FromEntry(dbEntry))
                .Should().Throw<ArgumentException>();
        }

        [Test]
        public void FromEntryTestWrongObjectsReturnsEmptyObjects()
        {
            var list = CreateObjectList<SomeOtherObject>();
            DynamoDBList dbEntry = new DynamoDBList(
                list.Select(x => Document.FromJson(JsonConvert.SerializeObject(x, new StringEnumConverter()))));

            var expected = new List<SomeObject>(new[]
            {
                new SomeObject(),
                new SomeObject(),
                new SomeObject(),
            });
            _sut.FromEntry(dbEntry).Should().BeEquivalentTo(expected);
        }
    }
}
