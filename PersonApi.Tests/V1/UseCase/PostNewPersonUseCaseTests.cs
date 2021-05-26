using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase;
using Xunit;

namespace PersonApi.Tests.V1.UseCase
{
    public class PostNewPersonUseCaseTests
    {
        private readonly Mock<IPersonApiGateway> _mockGateway;
        private readonly ResponseFactory _responseFactory;
        private readonly PostNewPersonUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        public PostNewPersonUseCaseTests()
        {
            _mockGateway = new Mock<IPersonApiGateway>();
            _responseFactory = new ResponseFactory(null);
            _classUnderTest = new PostNewPersonUseCase(_mockGateway.Object, _responseFactory);
        }

        [Fact]
        public async Task GetPersonByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var personRequest = new PersonRequestObject();
            var person = _fixture.Create<Person>();

            _mockGateway.Setup(x => x.PostNewPersonAsync(personRequest)).ReturnsAsync(person);

            // Act
            var response = await _classUnderTest.ExecuteAsync(personRequest)
                .ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(_responseFactory.ToResponse(person));
        }

        [Fact]
        public void GetPersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var personRequest = new PersonRequestObject();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.PostNewPersonAsync(personRequest)).ThrowsAsync(exception);

            // Act
            Func<Task<PersonResponseObject>> func = async () => await _classUnderTest.ExecuteAsync(personRequest).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
