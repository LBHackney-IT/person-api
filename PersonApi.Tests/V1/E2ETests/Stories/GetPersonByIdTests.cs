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
        IWant = "an endpoint to return person details",
        SoThat = "it is possible to view the details of a person")]
    [Collection("GetPersonByIdTests")]
    [CollectionDefinition("GetPersonByIdTests", DisableParallelization = true)]
    public class GetPersonByIdTests : IClassFixture<DynamoDbIntegrationTests<Startup>>, IDisposable
    {
        private readonly DynamoDbIntegrationTests<Startup> _classFixture;
        private readonly PersonFixture _personFixture;
        private readonly GetPersonSteps _steps;

        public GetPersonByIdTests(DynamoDbIntegrationTests<Startup> classFixture)
        {
            _classFixture = classFixture;
            _classFixture.CreateClient();
            _personFixture = new PersonFixture(_classFixture.DynamoDbContext);
            _steps = new GetPersonSteps(_classFixture.Client);
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
            this.Given(g => _personFixture.GivenAPersonAlreadyExists())
                .When(w => _steps.WhenThePersonDetailsAreRequested(_personFixture.PersonId.ToString()))
                .Then(t => _steps.ThenThePersonDetailsAreReturned(_personFixture.Person))
                .BDDfy();
        }

        [Fact]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsNotFoundIfPersonNotExist()
        {
            this.Given(g => _personFixture.GivenAPersonDoesNotExist())
                .When(w => _steps.WhenThePersonDetailsAreRequested(_personFixture.PersonId.ToString()))
                .Then(t => _steps.ThenNotFoundIsReturned())
                .BDDfy();
        }

        [Fact]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsBadRequestIfIdInvalid()
        {
            this.Given(g => _personFixture.GivenAnInvalidPersonId())
                .When(w => _steps.WhenThePersonDetailsAreRequested(_personFixture.InvalidPersonId))
                .Then(t => _steps.ThenBadRequestIsReturned())
                .BDDfy();
        }
    }
}
