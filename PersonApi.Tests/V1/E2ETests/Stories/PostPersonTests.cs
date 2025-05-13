using Hackney.Core.Testing.DynamoDb;
using Hackney.Core.Testing.Sns;
using Hackney.Shared.Person.Domain;
using PersonApi.Tests.V1.E2ETests.Fixtures;
using PersonApi.Tests.V1.E2ETests.Steps;
using System;
using TestStack.BDDfy;
using Xunit;

namespace PersonApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "an endpoint to create a new person",
        SoThat = "it is possible to create the details of a person")]
    [Collection("AppTest collection")]
    public class PostPersonTests : IDisposable
    {
        private readonly IDynamoDbFixture _dbFixture;
        private readonly ISnsFixture _snsFixture;
        private readonly PersonFixture _personFixture;
        private readonly PostPersonSteps _steps;

        public PostPersonTests(MockWebApplicationFactory<Startup> appFactory)
        {
            _dbFixture = appFactory.DynamoDbFixture;
            _snsFixture = appFactory.SnsFixture;
            _personFixture = new PersonFixture(_dbFixture.DynamoDbContext, _snsFixture.SimpleNotificationService);
            _steps = new PostPersonSteps(appFactory.Client);
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
        public void ServiceReturnsTheRequestedPerson()
        {
            this.Given(g => _personFixture.GivenANewPersonRequest())
                .Given(g => _personFixture.CreateRefGenerator())
                .When(w => _steps.WhenTheCreatePersonApiIsCalled(_personFixture.CreatePersonRequest))
                .Then(t => _steps.ThenThePersonDetailsAreReturnedAndIdIsNotEmpty(_personFixture))
                .Then(t => _steps.ThenPersonRefIsUpdatedInRefGeneratorTable(_personFixture, _personFixture.PersonRef))
                .Then(t => _steps.ThenThePersonCreatedEventIsRaised(_snsFixture))
                .BDDfy();
        }

        [Theory]
        [InlineData(PersonType.HousingOfficer)]
        [InlineData(PersonType.HousingAreaManager)]
        public void ServiceReturnsTheRequestedPersonWithoutDOBAndTitle(PersonType personType)
        {
            this.Given(g => _personFixture.GivenANewPersonRequestWithoutDOBAndTitle(personType))
                .Given(g => _personFixture.CreateRefGenerator())
                .When(w => _steps.WhenTheCreatePersonApiIsCalled(_personFixture.CreatePersonRequest))
                .Then(t => _steps.ThenThePersonDetailsAreReturnedAndIdIsNotEmpty(_personFixture))
                .Then(t => _steps.ThenPersonRefIsUpdatedInRefGeneratorTable(_personFixture, _personFixture.PersonRef))
                .Then(t => _steps.ThenThePersonCreatedEventIsRaised(_snsFixture))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsBadRequestWhenThereAreValidationErrors()
        {
            this.Given(g => _personFixture.GivenANewPersonRequestWithValidationErrors())
                .Given(g => _personFixture.CreateRefGenerator())
                .When(w => _steps.WhenTheCreatePersonApiIsCalled(_personFixture.CreatePersonRequest))
                .Then(r => _steps.ThenBadRequestIsReturned())
                .Then(t => _steps.ThenPersonRefIsNotUpdatedInRefGeneratorTable(_personFixture, _personFixture.PersonRef))
                .And(t => _steps.ThenTheValidationErrorsAreReturned())
                .BDDfy();
        }
    }
}
