using System;
using System.Collections.Generic;

namespace Hackney.Core.Testing.PactBroker
{
    public delegate void PactStateHandler(string state, IDictionary<string, string> args);

    public interface IPactBrokerHandler : IDisposable
    {
        IDictionary<string, PactStateHandler> ProviderStates { get; }
    }
}
