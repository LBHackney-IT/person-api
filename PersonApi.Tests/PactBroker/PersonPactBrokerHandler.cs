using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using Hackney.Core.Testing.PactBroker;
using Hackney.Shared.Person.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonApi.Tests.PactBroker
{
    public class PersonPactBrokerHandler : IPactBrokerHandler
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly IDynamoDBContext _dynamoDbContext;
        private static readonly List<Action> _cleanup = new List<Action>();

        public IDictionary<string, PactStateHandler> ProviderStates { get; private set; }

        public PersonPactBrokerHandler(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDbContext = dynamoDBContext;

            ProviderStates = new Dictionary<string, PactStateHandler>
            {
                {
                    "the Person API has a person with an id:6fbe024f-2316-4265-a6e8-d65a837e308a",
                    ARecordExists
                }
            };
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                foreach (var act in _cleanup)
                    act();
                _cleanup.Clear();

                _disposed = true;
            }
        }

        public void ARecordExists(string name, IDictionary<string, string> parameters)
        {
            string id = null;
            if (parameters.ContainsKey("id"))
                id = parameters["id"];
            else
                id = name.Split(':')?.Last();

            if (!string.IsNullOrEmpty(id))
            {
                var testPerson = _fixture.Build<PersonDbEntity>()
                                         .With(x => x.Id, Guid.Parse(id))
                                         .With(x => x.VersionNumber, (int?) null)
                                         .Create();
                _dynamoDbContext.SaveAsync<PersonDbEntity>(testPerson).GetAwaiter().GetResult();

                _cleanup.Add(async () => await _dynamoDbContext.DeleteAsync<PersonDbEntity>(testPerson).ConfigureAwait(false));
            }
        }
    }
}
