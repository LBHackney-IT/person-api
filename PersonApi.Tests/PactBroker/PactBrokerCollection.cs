using Xunit;

namespace PersonApi.Tests.PactBroker
{
    [CollectionDefinition("Pact Broker collection", DisableParallelization = true)]
    public class PactBrokerCollection : ICollectionFixture<PersonPactBrokerFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
