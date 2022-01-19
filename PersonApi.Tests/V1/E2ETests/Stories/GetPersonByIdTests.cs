using Hackney.Core.Testing.DynamoDb;
using PersonApi.Tests.V1.E2ETests.Fixtures;
using PersonApi.Tests.V1.E2ETests.Steps;
using System;
using TestStack.BDDfy;
using Xunit;

namespace PersonApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "an endpoint to return person details",
        SoThat = "it is possible to view the details of a person")]
    [Collection("AppTest collection")]
    public class GetPersonByIdTests : IDisposable
    {
        private readonly IDynamoDbFixture _dbFixture;
        private readonly PersonFixture _personFixture;
        private readonly GetPersonSteps _steps;

        public GetPersonByIdTests(MockWebApplicationFactory<Startup> appFactory)
        {
            _dbFixture = appFactory.DynamoDbFixture;
            _personFixture = new PersonFixture(_dbFixture.DynamoDbContext, appFactory.SnsFixture.SimpleNotificationService);
            _steps = new GetPersonSteps(appFactory.Client);
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
            this.Given(g => _personFixture.GivenAPersonAlreadyExists())
                .When(w => _steps.WhenThePersonDetailsAreRequested(_personFixture.PersonId.ToString()))
                .Then(t => _steps.ThenThePersonDetailsAreReturned(_personFixture.Person))
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsNotFoundIfPersonNotExist()
        {
            this.Given(g => _personFixture.GivenAPersonDoesNotExist())
                .When(w => _steps.WhenThePersonDetailsAreRequested(_personFixture.PersonId.ToString()))
                .Then(t => _steps.ThenNotFoundIsReturned())
                .BDDfy();
        }

        [Fact]
        public void ServiceReturnsBadRequestIfIdInvalid()
        {
            this.Given(g => _personFixture.GivenAnInvalidPersonId())
                .When(w => _steps.WhenThePersonDetailsAreRequested(_personFixture.InvalidPersonId))
                .Then(t => _steps.ThenBadRequestIsReturned())
                .BDDfy();
        }
    }
}
