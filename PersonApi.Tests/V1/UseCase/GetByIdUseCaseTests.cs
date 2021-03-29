using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using PersonApi.V1.Domain;
using FluentAssertions;
using AutoFixture;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;

namespace PersonApi.Tests.V1.UseCase
{
    public class GetByIdUseCaseTests
    {
        private Mock<IPersonApiGateway> _mockGateway;
        private ResponseFactory _responseFactory;
        private GetByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IPersonApiGateway>();
            _responseFactory = new ResponseFactory(null);
            _classUnderTest = new GetByIdUseCase(_mockGateway.Object, _responseFactory);
        }

        [Test]
        public async Task GetByIdUseCaseGatewayReturnsNullReturnsNull()
        {
            var id = Guid.NewGuid();
            _mockGateway.Setup(x => x.GetPersonByIdAsync(id)).ReturnsAsync((Person) null);
            var response = await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            response.Should().BeNull();
        }

        [Test]
        public async Task GetPersonByIdAsyncFoundReturnsResponse()
        {
            var id = Guid.NewGuid();
            var person = _fixture.Create<Person>();
            _mockGateway.Setup(x => x.GetPersonByIdAsync(id)).ReturnsAsync(person);
            var response = await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            response.Should().BeEquivalentTo(_responseFactory.ToResponse(person));
        }

        [Test]
        public void GetPersonByIdAsyncExceptionIsThrown()
        {
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetPersonByIdAsync(id)).ThrowsAsync(exception);
            Func<Task<PersonResponseObject>> func = async () => await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
