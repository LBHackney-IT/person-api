using System.Collections.Generic;

namespace PersonApi.Tests.PactBroker
{
    public class ProviderState
    {
        public string State { get; set; }
        public IDictionary<string, string> Params { get; set; }
    }
}
