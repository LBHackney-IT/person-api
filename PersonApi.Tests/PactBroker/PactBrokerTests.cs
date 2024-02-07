using Hackney.Core.Testing.PactBroker;
using PactNet.Infrastructure.Outputters;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace PersonApi.Tests.PactBroker
{
    [Collection("Pact Broker collection")]
    public class PactBrokerTests
    {
        private readonly PersonPactBrokerFixture _testFixture;
        private readonly ITestOutputHelper _outputHelper;

        public PactBrokerTests(PersonPactBrokerFixture testFixture, ITestOutputHelper output)
        {
            _testFixture = testFixture;
            _outputHelper = output;
        }

        //[Fact]
        //public void EnsureTheApiHonoursPactWithConsumer()
        //{
        //    _testFixture.RunPactBrokerTest(new List<IOutput> { new XUnitOutput(_outputHelper) });
        //}
    }
}
