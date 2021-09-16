using AutoFixture;
using FluentAssertions;
using Hackney.Core.JWT;
using Hackney.Shared.Person;
using PersonApi.V2.Factories;
using PersonApi.V2.Infrastructure;
using System;
using Xunit;

namespace PersonApi.Tests.V2.Factories
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
            result.Version.Should().Be(CreateEventConstants.V2VERSION);
        }
    }
}
