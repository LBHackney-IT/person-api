using PersonApi.Tests.V1.E2ETests.Fixtures;
using PersonApi.Tests.V1.E2ETests.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Xunit;

namespace PersonApi.Tests.V1.E2ETests.Stories
{
    [Story(
            AsA = "Internal Hackney user (such as a Housing Officer or Area housing Manager)",
            IWant = "to be able to amend a persons details",
            SoThat = "customer details are kept up to date")]
    [Collection("Aws collection")]
    public class UpdatePersonTests : IDisposable
    {
        private readonly AwsIntegrationTests<Startup> _dbFixture;
        private readonly PersonFixture _personFixture;
        private readonly UpdatePersonStep _steps;

        public UpdatePersonTests(AwsIntegrationTests<Startup> dbFixture)
        {
            _dbFixture = dbFixture;
            _personFixture = new PersonFixture(_dbFixture.DynamoDbContext, _dbFixture.SimpleNotificationService);
            _steps = new UpdatePersonStep(_dbFixture.Client);
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
            this.Given(g => _personFixture.GivenAPersonAlreadyExistsAndUpdateRequested())
                .And(g => _personFixture.GivenAUpdatePersonRequest(_personFixture.Person))
                .When(w => _steps.WhenTheUpdatePersonApiIsCalled(_personFixture.UpdatePersonRequest))
                .Then(t => _steps.ThenThePersonDetailsAreUpdated(_personFixture))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsBadRequest()
        {
            this.Given(g => _personFixture.GivenAPersonIdDoesNotExist())
                .And(g => _personFixture.GivenAUpdatePersonRequest(_personFixture.Person))
                .When(w => _steps.WhenTheUpdatePersonApiIsCalled(_personFixture.UpdatePersonRequest))
                .Then(r => _steps.ThenBadRequestIsReturned())
                .BDDfy();
        }
    }
}
