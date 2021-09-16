using AutoFixture;
using FluentAssertions;
using Moq;
using PersonApi.V1.Boundary.Request;
using Hackney.Shared.Person;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.UseCase
{
    [Collection("LogCall collection")]
    public class GetByIdUseCaseTests
    {
        private readonly Mock<IPersonApiGateway> _mockGateway;
        private readonly GetByIdUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        public GetByIdUseCaseTests()
        {
            _mockGateway = new Mock<IPersonApiGateway>();
            _classUnderTest = new GetByIdUseCase(_mockGateway.Object);
        }

        private PersonQueryObject ConstructQuery()
        {
            return new PersonQueryObject() { Id = Guid.NewGuid() };
        }

        [Fact]
        public async Task GetByIdUseCaseGatewayReturnsNullReturnsNull()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGateway.Setup(x => x.GetPersonByIdAsync(query)).ReturnsAsync((Person) null);

            // Act
            var response = await _classUnderTest.ExecuteAsync(query).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var query = ConstructQuery();
            var person = _fixture.Create<Person>();
            _mockGateway.Setup(x => x.GetPersonByIdAsync(query)).ReturnsAsync(person);

            // Act
            var response = await _classUnderTest.ExecuteAsync(query).ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(person);
        }

        [Fact]
        public void GetPersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var query = ConstructQuery();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.GetPersonByIdAsync(query)).ThrowsAsync(exception);

            // Act
            Func<Task<Person>> func = async () => await _classUnderTest.ExecuteAsync(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
