using PersonApi.Tests.V1.E2ETests.Fixtures;
using PersonApi.Tests.V1.E2ETests.Steps;
using System;
using System.Diagnostics.CodeAnalysis;
using TestStack.BDDfy;
using Xunit;

namespace PersonApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "an endpoint to create a new person",
        SoThat = "it is possible to create the details of a person")]
    [Collection("DynamoDb collection")]
    public class PostPersonTests : IDisposable
    {
        private readonly DynamoDbIntegrationTests<Startup> _dbFixture;
        private readonly PersonFixture _personFixture;
        private readonly PostPersonSteps _steps;

        public PostPersonTests(DynamoDbIntegrationTests<Startup> dbFixture)
        {
            _dbFixture = dbFixture;
            _personFixture = new PersonFixture(_dbFixture.DynamoDbContext, _dbFixture.SimpleNotificationService);
            _steps = new PostPersonSteps(_dbFixture.Client);
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
                if (null != _personFixture)
                    _personFixture.Dispose();

                _disposed = true;
            }
        }

        [Fact]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsTheRequestedPerson()
        {
            this.Given(g => _personFixture.GivenANewPersonIsCreated())
                .When(w => _steps.WhenAPersonIsCreated(_personFixture.PersonRequest))
                .Then(t => _steps.ThenThePersonDetailsAreReturnedAndIdIsNotEmpty())
                .BDDfy();
        }
    }
}
