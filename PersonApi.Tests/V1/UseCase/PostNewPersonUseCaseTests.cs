using AutoFixture;
using FluentAssertions;
using Hackney.Core.JWT;
using Moq;
using PersonApi.V1.Boundary.Request;
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
    public class PostNewPersonUseCaseTests
    {
        private readonly Mock<IPersonApiGateway> _mockGateway;
        private readonly ResponseFactory _responseFactory;
        private readonly Mock<ISnsGateway> _personSnsGateway;
        private readonly PersonSnsFactory _personSnsFactory;
        private readonly PostNewPersonUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        public PostNewPersonUseCaseTests()
        {
            _mockGateway = new Mock<IPersonApiGateway>();
            _responseFactory = new ResponseFactory(null);
            _personSnsGateway = new Mock<ISnsGateway>();
            _personSnsFactory = new PersonSnsFactory();
            _classUnderTest = new PostNewPersonUseCase(_mockGateway.Object, _responseFactory, _personSnsGateway.Object, _personSnsFactory);

            _personSnsGateway.Setup(x => x.Publish(It.IsAny<PersonSns>()));
        }

        [Fact]
        public async Task CreatePersonByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var personRequest = new CreatePersonRequestObject();
            var token = new Token();

            var person = _fixture.Create<Person>();

            _mockGateway.Setup(x => x.PostNewPersonAsync(personRequest)).ReturnsAsync(person);

            // Act
            var response = await _classUnderTest.ExecuteAsync(personRequest, token)
                .ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(_responseFactory.ToResponse(person));
        }

        [Fact]
        public void CreatePersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var personRequest = new CreatePersonRequestObject();
            var token = new Token();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.PostNewPersonAsync(personRequest)).ThrowsAsync(exception);

            // Act
            Func<Task<PersonResponseObject>> func = async () => await _classUnderTest.ExecuteAsync(personRequest, token).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
