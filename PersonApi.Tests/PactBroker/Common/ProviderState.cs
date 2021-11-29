using System.Collections.Generic;

namespace Hackney.Core.Testing.PactBroker
{
    public class ProviderState
    {
        public string State { get; set; }
        public IDictionary<string, string> Params { get; set; }
    }
}
