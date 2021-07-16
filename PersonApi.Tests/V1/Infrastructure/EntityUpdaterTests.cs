using AutoFixture;
using FluentAssertions;
using Force.DeepCloner;
using Hackney.Core;
using Microsoft.Extensions.Logging;
using Moq;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace PersonApi.Tests.V1.Infrastructure
{
    public class EntityUpdaterTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ILogger<EntityUpdater>> _mockLogger;
        private readonly EntityUpdater _sut;

        private readonly JsonSerializerOptions _jsonOptions;
        private const string NotValidJson = "Not valid json";
        private const string JsonWithUnknownPropertyName = "{ \"unknownProp\": \"Some new value\" }";
        private const string JsonWithKnownPropertyNameNotOnRequestObj = "{ \"someOtherString\": \"Some new value\" }";

        public EntityUpdaterTests()
        {
            _mockLogger = new Mock<ILogger<EntityUpdater>>();
            _jsonOptions = CreateJsonOptions();
            _sut = new EntityUpdater(_mockLogger.Object);
        }

        private static JsonSerializerOptions CreateJsonOptions(bool ignoreNulls = false)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            if (ignoreNulls)
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            return options;
        }

        private static void VerifyValues(Dictionary<string, object> expected, Dictionary<string, object> actual)
        {
            actual.Should().HaveCount(expected.Count);
            foreach (var prop in expected)
            {
                var expectedVal = prop.Value;
                if (expectedVal is null) actual[prop.Key].Should().BeNull();
                else expectedVal.Equals(actual[prop.Key]).Should().BeTrue();
            }
        }

        private static Dictionary<string, object> ConstructJsonDictionaryFromObject(object obj, params string[] includePropNames)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var propNameCamelCase = prop.Name.ToCamelCase();
                if ((null == includePropNames) || (includePropNames.Length == 0) || includePropNames.Contains(propNameCamelCase))
                    dic.Add(propNameCamelCase, prop.GetValue(obj));
            }
            return dic;
        }

        [Fact]
        public void UpdateEntityNullEntityThrows()
        {
            Func<UpdateEntityResult<Entity>> func = () => _sut.UpdateEntity((Entity) null, NotValidJson, new EntityUpdateRequest());
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UpdateEntityNullUpdateRequestThrows()
        {
            Func<UpdateEntityResult<Entity>> func = () => _sut.UpdateEntity(new Entity(), NotValidJson, (EntityUpdateRequest) null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UpdateEntityInvalidUpdateJsonThrows()
        {
            Func<UpdateEntityResult<Entity>> func = () => _sut.UpdateEntity(new Entity(), NotValidJson, new EntityUpdateRequest());
            func.Should().Throw<JsonException>();
        }

        [Fact]
        public void UpdateEntityJsonContainsUnknownPropertyNameIgnoresAndLogsWarning()
        {
            var result = _sut.UpdateEntity(new Entity(), JsonWithUnknownPropertyName, new EntityUpdateRequest());

            result.IgnoredProperties.Count.Should().Be(1);
            result.IgnoredProperties.First().Should().Be("unknownProp");
            _mockLogger.VerifyExact(LogLevel.Warning,
                $"Entity object (type: {typeof(Entity).Name}) does not contain a property called unknownProp. Ignoring unknownProp value...",
                Times.Once());
        }

        [Fact]
        public void UpdateEntityUpdateRequestContainsUnknownPropertyNameIgnoresAndLogsWarning()
        {
            var requestObj = new OtherEntityUpdateRequest() { UnknownProp = "Some new value" };
            var result = _sut.UpdateEntity(new Entity(), JsonWithKnownPropertyNameNotOnRequestObj, requestObj);

            result.IgnoredProperties.Count.Should().Be(1);
            result.IgnoredProperties.First().Should().Be("someOtherString");
            _mockLogger.VerifyExact(LogLevel.Warning,
                $"Request object (type: {typeof(OtherEntityUpdateRequest).Name}) does not contain a property called SomeOtherString that is on the entity type ({typeof(Entity).Name}). Ignoring SomeOtherString value...",
                Times.Once());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("{ }")]
        public void UpdateEntityNoUpdateJsonReturnsResult(string inputJson)
        {
            var entity = _fixture.Create<Entity>();
            var requestObject = new EntityUpdateRequest();
            var result = _sut.UpdateEntity(entity, inputJson, requestObject);
            result.Should().NotBeNull();
            result.UpdatedEntity.Should().Be(entity);
            result.OldValues.Should().HaveCount(0);
            result.NewValues.Should().HaveCount(0);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpdateEntityWithSameValuesReturnsResult(bool ignoreUnchanged)
        {
            var entity = _fixture.Create<Entity>();
            var requestObject = new EntityUpdateRequest();
            requestObject.ASubEntity = entity.ASubEntity;
            requestObject.SomeBool = entity.SomeBool;
            requestObject.SomeDate = entity.SomeDate;
            requestObject.SomeEnum = entity.SomeEnum;
            requestObject.SomeInt = entity.SomeInt;
            requestObject.SomeString = entity.SomeString;
            var inputJson = JsonSerializer.Serialize(requestObject, _jsonOptions);

            var result = _sut.UpdateEntity(entity, inputJson, requestObject, ignoreUnchanged);
            result.Should().NotBeNull();
            result.UpdatedEntity.Should().Be(entity);

            Dictionary<string, object> expected = ignoreUnchanged ?
                  new Dictionary<string, object>() : ConstructJsonDictionaryFromObject(requestObject);

            VerifyValues(expected, result.OldValues);
            VerifyValues(expected, result.NewValues);
        }

        [Fact]
        public void UpdateEntityWithAllDifferentValuesReturnsResult()
        {
            var entity = _fixture.Build<Entity>().With(x => x.SomeBool, true).Create();
            var requestObject = _fixture.Build<EntityUpdateRequest>().With(x => x.SomeBool, false).Create();
            var inputJson = JsonSerializer.Serialize(requestObject, _jsonOptions);

            var propNames = requestObject.GetType().GetProperties().Select(x => x.Name.ToCamelCase());
            var expectedOld = ConstructJsonDictionaryFromObject(entity, propNames.ToArray());
            var expectedNew = ConstructJsonDictionaryFromObject(requestObject, propNames.ToArray());

            var result = _sut.UpdateEntity(entity, inputJson, requestObject);
            result.Should().NotBeNull();
            result.UpdatedEntity.Should().Be(entity);

            VerifyValues(expectedOld, result.OldValues);
            VerifyValues(expectedNew, result.NewValues);
        }

        [Fact]
        public void UpdateEntityWithSomeDifferentValuesReturnsResult()
        {
            var entity = _fixture.Build<Entity>().With(x => x.SomeBool, true).Create();
            var requestObject = _fixture.Build<EntityUpdateRequest>()
                                        // Updated
                                        .With(x => x.SomeBool, !entity.SomeBool)
                                        .With(x => x.SomeString, "A new string")
                                        .With(x => x.SomeDate, DateTime.UtcNow)
                                        .With(x => x.ASubEntity, entity.ASubEntity.DeepClone())
                                        // The same
                                        .With(x => x.SomeEnum, (EnumExample?) null)
                                        .With(x => x.SomeInt, (int?) null)
                                        .Create();
            requestObject.ASubEntity.AString = "Some sub object string";
            var inputJson = JsonSerializer.Serialize(requestObject, CreateJsonOptions(true));

            var propNames = new string[] { "someBool", "someString", "someDate", "aSubEntity" };
            var expectedOld = ConstructJsonDictionaryFromObject(entity, propNames.ToArray());
            var expectedNew = ConstructJsonDictionaryFromObject(requestObject, propNames.ToArray());

            var result = _sut.UpdateEntity(entity, inputJson, requestObject);
            result.Should().NotBeNull();
            result.UpdatedEntity.Should().Be(entity);

            VerifyValues(expectedOld, result.OldValues);
            VerifyValues(expectedNew, result.NewValues);
        }

        [Fact]
        public void UpdateEntityWithSomeNullValuesReturnsResult()
        {
            var entity = _fixture.Build<Entity>().With(x => x.SomeBool, true).Create();
            var requestObject = _fixture.Build<EntityUpdateRequest>()
                                        // Updated
                                        .With(x => x.SomeBool, !entity.SomeBool)
                                        .With(x => x.SomeString, (string) null)
                                        .With(x => x.SomeDate, DateTime.UtcNow)
                                        .With(x => x.ASubEntity, (SubEntity) null)
                                        .With(x => x.SomeEnum, (EnumExample?) null)
                                        .With(x => x.SomeInt, (int?) null)
                                        .Create();
            var inputJson = JsonSerializer.Serialize(requestObject, CreateJsonOptions(false));

            var propNames = new string[] { "someBool", "someString", "someDate", "aSubEntity", "someEnum", "someInt" };
            var expectedOld = ConstructJsonDictionaryFromObject(entity, propNames.ToArray());
            var expectedNew = ConstructJsonDictionaryFromObject(requestObject, propNames.ToArray());

            var result = _sut.UpdateEntity(entity, inputJson, requestObject);
            result.Should().NotBeNull();
            result.UpdatedEntity.Should().Be(entity);

            VerifyValues(expectedOld, result.OldValues);
            VerifyValues(expectedNew, result.NewValues);
        }
    }

    public enum EnumExample { one, two, three, four, five }

    public class Entity
    {
        public Guid Id { get; set; }
        public EnumExample SomeEnum { get; set; }
        public string SomeString { get; set; }
        public int SomeInt { get; set; }
        public bool SomeBool { get; set; }
        public DateTime SomeDate { get; set; }
        public SubEntity ASubEntity { get; set; }
        public string SomeOtherString { get; set; }
    }

    public class SubEntity
    {
        public string AString { get; set; }
        public bool ABool { get; set; }
        public string AnotherString { get; set; }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType()) return false;
            var otherObj = (SubEntity) obj;
            return ABool.Equals(otherObj.ABool)
                && AnotherString.Equals(otherObj.AnotherString)
                && AString.Equals(otherObj.AString);
        }

        public override int GetHashCode()
        {
            return (AString + ABool.ToString() + AnotherString).GetHashCode();
        }
    }

    public class EntityUpdateRequest
    {
        public EnumExample? SomeEnum { get; set; }
        public string SomeString { get; set; }
        public int? SomeInt { get; set; }
        public bool? SomeBool { get; set; }
        public DateTime? SomeDate { get; set; }
        public SubEntity ASubEntity { get; set; }
    }

    public class OtherEntityUpdateRequest
    {
        public string UnknownProp { get; set; }
    }
}
