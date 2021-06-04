using PersonApi.Tests.V1.E2ETests.Steps;
using System;
using TestStack.BDDfy;
using Xunit;

namespace PersonApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Api client",
        IWant = "to be able to validate that the service satus is healty",
        SoThat = "I can be sure that calls made to it will succeed.")]
    [Collection("Aws collection")]
    public class HealthCheckTests : IDisposable
    {
        private readonly AwsIntegrationTests<Startup> _dbFixture;
        private readonly HealthCheckSteps _steps;

        public HealthCheckTests(AwsIntegrationTests<Startup> dbFixture)
        {
            _dbFixture = dbFixture;
            _steps = new HealthCheckSteps(_dbFixture.Client);
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
                _disposed = true;
            }
        }

        [Fact]
        public void ServiceReturnsHealthyStatus()
        {
            this.Given("A running service")
                .When(w => _steps.WhenTheHealtchCheckIsCalled())
                .Then(t => _steps.ThenTheHealthyStatusIsReturned())
                .BDDfy();
        }
    }
}
