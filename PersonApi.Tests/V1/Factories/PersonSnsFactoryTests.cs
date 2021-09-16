using AutoFixture;
using FluentAssertions;
using Hackney.Core.JWT;
using Hackney.Shared.Person;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace PersonApi.Tests.V1.Factories
{
    public class PersonSnsFactoryTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void CreateTestCreatesSnsEvent()
        {
            var person = _fixture.Create<Person>();
            var token = _fixture.Create<Token>();

            var expectedEventData = new EventData() { NewData = person };
            var expectedUser = new User() { Email = token.Email, Name = token.Name };

            var factory = new PersonSnsFactory();
            var result = factory.Create(person, token);

            result.CorrelationId.Should().NotBeEmpty();
            result.DateTime.Should().BeCloseTo(DateTime.UtcNow, 100);
            result.EntityId.Should().Be(person.Id);
            result.EventData.Should().BeEquivalentTo(expectedEventData);
            result.EventType.Should().Be(CreateEventConstants.EVENTTYPE);
            result.Id.Should().NotBeEmpty();
            result.SourceDomain.Should().Be(CreateEventConstants.SOURCEDOMAIN);
            result.SourceSystem.Should().Be(CreateEventConstants.SOURCESYSTEM);
            result.User.Should().BeEquivalentTo(expectedUser);
            result.Version.Should().Be(CreateEventConstants.V1VERSION);
        }

        [Fact]
        public void UpdateTestCreatesSnsEvent()
        {
            var personDb = _fixture.Create<PersonDbEntity>();
            var updateResult = _fixture.Build<UpdateEntityResult<PersonDbEntity>>()
                                       .With(x => x.UpdatedEntity, personDb)
                                       .With(x => x.OldValues, new Dictionary<string, object> { { "title", "Dr" } })
                                       .With(x => x.NewValues, new Dictionary<string, object> { { "title", "Mr" } })
                                       .Create();
            var token = _fixture.Create<Token>();

            var expectedEventData = new EventData() { NewData = updateResult.NewValues, OldData = updateResult.OldValues };
            var expectedUser = new User() { Email = token.Email, Name = token.Name };

            var factory = new PersonSnsFactory();
            var result = factory.Update(updateResult, token);

            result.CorrelationId.Should().NotBeEmpty();
            result.DateTime.Should().BeCloseTo(DateTime.UtcNow, 100);
            result.EntityId.Should().Be(personDb.Id);
            result.EventData.Should().BeEquivalentTo(expectedEventData);
            result.EventType.Should().Be(UpdatePersonConstants.EVENTTYPE);
            result.Id.Should().NotBeEmpty();
            result.SourceDomain.Should().Be(UpdatePersonConstants.SOURCEDOMAIN);
            result.SourceSystem.Should().Be(UpdatePersonConstants.SOURCESYSTEM);
            result.User.Should().BeEquivalentTo(expectedUser);
            result.Version.Should().Be(UpdatePersonConstants.V1VERSION);
        }
    }
}
