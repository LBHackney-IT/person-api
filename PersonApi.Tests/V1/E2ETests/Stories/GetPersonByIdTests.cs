using NUnit.Framework;
using PersonApi.Tests.V1.E2ETests.Fixtures;
using PersonApi.Tests.V1.E2ETests.Steps;
using System.Diagnostics.CodeAnalysis;
using TestStack.BDDfy;

namespace PersonApi.Tests.V1.E2ETests.Stories
{
    [Story(
        AsA = "Service",
        IWant = "an endpoint to return person details",
        SoThat = "it is possible to view the details of a person")]
    [TestFixture]
    public class GetPersonByIdTests : DynamoDbIntegrationTests<Startup>
    {
        private PersonFixture _personFixture;
        private GetPersonSteps _steps;

        [SetUp]
        public void Setup()
        {
            _personFixture = new PersonFixture(DynamoDbContext);
            _steps = new GetPersonSteps(Client);
        }

        [TearDown]
        public void TearDown()
        {
            _personFixture.Dispose();
        }

        [Test]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsTheRequestedPerson()
        {
            this.Given(g => _personFixture.GivenAPersonAlreadyExists())
                .When(w => _steps.WhenThePersonDetailsAreRequested(_personFixture.PersonId.ToString()))
                .Then(t => _steps.ThenThePersonDetailsAreReturned(_personFixture.Person))
                .BDDfy();
        }

        [Test]
        [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "BDDfy")]
        public void ServiceReturnsNotFoundIfPersonNotExist()
        {
            this.Given(g => _personFixture.GivenAPersonDoesNotExist())
                .When(w => _steps.WhenThePersonDetailsAreRequested(_personFixture.PersonId.ToString()))
                .Then(t => _steps.ThenNotFoundIsReturned())
                .BDDfy();
        }

        [Test]
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
