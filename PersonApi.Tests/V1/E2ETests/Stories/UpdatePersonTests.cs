using Hackney.Core.Testing.DynamoDb;
using Hackney.Core.Testing.Sns;
using PersonApi.Tests.V1.E2ETests.Fixtures;
using PersonApi.Tests.V1.E2ETests.Steps;
using System;
using TestStack.BDDfy;
using Xunit;

namespace PersonApi.Tests.V1.E2ETests.Stories
{
    [Story(
            AsA = "Internal Hackney user (such as a Housing Officer or Area housing Manager)",
            IWant = "to be able to amend a persons details",
            SoThat = "customer details are kept up to date")]
    [Collection("AppTest collection")]
    public class UpdatePersonTests : IDisposable
    {
        private readonly IDynamoDbFixture _dbFixture;
        private readonly ISnsFixture _snsFixture;
        private readonly PersonFixture _personFixture;
        private readonly UpdatePersonStep _steps;

        public UpdatePersonTests(MockWebApplicationFactory<Startup> appFactory)
        {
            _dbFixture = appFactory.DynamoDbFixture;
            _snsFixture = appFactory.SnsFixture;
            _personFixture = new PersonFixture(_dbFixture.DynamoDbContext, _snsFixture.SimpleNotificationService);
            _steps = new UpdatePersonStep(appFactory.Client);
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
                _personFixture?.Dispose();
                _snsFixture?.PurgeAllQueueMessages();

                _disposed = true;
            }
        }

        [Fact]
        public void ServiceUpdatesTheRequestedPerson()
        {
            this.Given(g => _personFixture.GivenAPersonAlreadyExistsAndUpdateRequested())
                .And(g => _personFixture.GivenAUpdatePersonRequest())
                .When(w => _steps.WhenTheUpdatePersonApiIsCalled(_personFixture.UpdatePersonRequest, _personFixture.PersonId))
                .Then(t => _steps.ThenThePersonDetailsAreUpdated(_personFixture))
                .Then(t => _steps.ThenThePersonUpdatedEventIsRaised(_personFixture, _snsFixture))
                .BDDfy();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        public void ServiceReturnsConflictWhenIncorrectVersionNumber(int? versionNumber)
        {
            this.Given(g => _personFixture.GivenAPersonAlreadyExistsAndUpdateRequested())
                .And(g => _personFixture.GivenAUpdatePersonRequest())
                .When(w => _steps.WhenTheUpdatePersonApiIsCalled(_personFixture.UpdatePersonRequest, _personFixture.PersonId, versionNumber))
                .Then(t => _steps.ThenConflictIsReturned(versionNumber))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsNotFoundWhenNoPersonExists()
        {
            this.Given(g => _personFixture.GivenAPersonIdDoesNotExist())
                .And(g => _personFixture.GivenAUpdatePersonRequest())
                .When(w => _steps.WhenTheUpdatePersonApiIsCalled(_personFixture.UpdatePersonRequest, _personFixture.PersonId))
                .Then(r => _steps.ThenNotFoundIsReturned())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsBadRequest()
        {
            this.Given(g => _personFixture.GivenAPersonIdDoesNotExist())
                .And(g => _personFixture.GivenAUpdatePersonRequest())
                .When(w => _steps.WhenTheUpdatePersonApiIsCalled(_personFixture.UpdatePersonRequest, null))
                .Then(r => _steps.ThenBadRequestIsReturned())
                .BDDfy();
        }
    }
}
