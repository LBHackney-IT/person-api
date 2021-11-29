using Hackney.Core.Testing.Shared;
using Xunit;

namespace PersonApi.Tests
{
    [CollectionDefinition("LogCall collection")]
    public class LogCallAspectFixtureCollection : ICollectionFixture<LogCallAspectFixture>
    { }
}
