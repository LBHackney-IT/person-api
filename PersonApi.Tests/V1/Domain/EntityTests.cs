using System;
using PersonApi.V1.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace PersonApi.Tests.V1.Domain
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void EntitiesHaveAnId()
        {
            var entity = new Person();
            var id = Guid.NewGuid().ToString();
            entity.Id = id;
            entity.Id.Should().Be(id);
        }

        [Test]
        public void EntitiesHaveACreatedAt()
        {
            var entity = new Person();
            var date = new DateTime(2019, 02, 21);
            entity.CreatedAt = date;

            entity.CreatedAt.Should().BeSameDateAs(date);
        }
    }
}
