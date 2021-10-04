using AutoFixture;
using FluentAssertions;
using Hackney.Core.JWT;
using Hackney.Shared.Person;
using Hackney.Shared.Person.Boundary.Request;
using Hackney.Shared.Person.Boundary.Response;
using Hackney.Shared.Person.Factories;
using Moq;
using PersonApi.V1.Domain;
using PersonApi.V1.Gateways;
using PersonApi.V2.Factories;
using PersonApi.V2.UseCase;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V2.UseCase
{
    public class PostNewPersonUseCaseTests
    {
        private readonly Mock<IPersonApiGateway> _mockGateway;
        private readonly ResponseFactory _responseFactory;
        private readonly Mock<ISnsGateway> _mockSnsGateway;
        private readonly Mock<ISnsFactory> _mockSnsFactory;
        private readonly PostNewPersonUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        public PostNewPersonUseCaseTests()
        {
            _mockGateway = new Mock<IPersonApiGateway>();
            _responseFactory = new ResponseFactory(null);
            _mockSnsGateway = new Mock<ISnsGateway>();
            _mockSnsFactory = new Mock<ISnsFactory>();
            _classUnderTest = new PostNewPersonUseCase(_mockGateway.Object, _responseFactory, _mockSnsGateway.Object, _mockSnsFactory.Object);
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

            _mockSnsFactory.Verify(x => x.Create(person, token), Times.Once);
            _mockSnsGateway.Verify(x => x.Publish(It.IsAny<PersonSns>()), Times.Once);
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

            _mockSnsFactory.Verify(x => x.Create(It.IsAny<Person>(), It.IsAny<Token>()), Times.Never);
            _mockSnsGateway.Verify(x => x.Publish(It.IsAny<PersonSns>()), Times.Never);
        }
    }
}
