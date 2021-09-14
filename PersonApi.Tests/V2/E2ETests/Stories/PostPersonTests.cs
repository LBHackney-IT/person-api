using PersonApi.Tests.V1.E2ETests.Fixtures;
using PersonApi.Tests.V2.E2ETests.Steps;
using System;
using TestStack.BDDfy;
using Xunit;

namespace PersonApi.Tests.V2.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "an endpoint to create a new person",
        SoThat = "it is possible to create the details of a person")]
    [Collection("Aws collection")]
    public class PostPersonTests : IDisposable
    {
        private readonly AwsIntegrationTests<Startup> _dbFixture;
        private readonly PersonFixture _personFixture;
        private readonly PostPersonSteps _steps;

        public PostPersonTests(AwsIntegrationTests<Startup> dbFixture)
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
        public void ServiceReturnsTheRequestedPerson()
        {
            this.Given(g => _personFixture.GivenANewPersonRequest())
                .When(w => _steps.WhenTheCreatePersonApiIsCalled(_personFixture.CreatePersonRequest))
                .Then(t => _steps.ThenThePersonDetailsAreReturnedAndIdIsNotEmpty(_personFixture))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsBadRequestWhenThereAreValidationErrors()
        {
            this.Given(g => _personFixture.GivenANewPersonRequestWithValidationErrors())
                .When(w => _steps.WhenTheCreatePersonApiIsCalled(_personFixture.CreatePersonRequest))
                .Then(r => _steps.ThenBadRequestIsReturned())
                .And(t => _steps.ThenTheValidationErrorsAreReturned())
                .BDDfy();
        }
    }
}
