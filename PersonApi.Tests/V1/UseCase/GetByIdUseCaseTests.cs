using AutoFixture;
using FluentAssertions;
using Moq;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.UseCase
{
    public class GetByIdUseCaseTests
    {
        private readonly Mock<IPersonApiGateway> _mockGateway;
        private readonly ResponseFactory _responseFactory;
        private readonly GetByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        public GetByIdUseCaseTests()
        {
            _mockGateway = new Mock<IPersonApiGateway>();
            _responseFactory = new ResponseFactory(null);
            _classUnderTest = new GetByIdUseCase(_mockGateway.Object, _responseFactory);
        }

        [Fact]
        public async Task GetByIdUseCaseGatewayReturnsNullReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockGateway.Setup(x => x.GetPersonByIdAsync(id)).ReturnsAsync((Person) null);

            // Act
            var response = await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var person = _fixture.Create<Person>();
            _mockGateway.Setup(x => x.GetPersonByIdAsync(id)).ReturnsAsync(person);

            // Act
            var response = await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(_responseFactory.ToResponse(person));
        }

        [Fact]
        public void GetPersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetPersonByIdAsync(id)).ThrowsAsync(exception);

            // Act
            Func<Task<PersonResponseObject>> func = async () => await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
