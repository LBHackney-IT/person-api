using AutoFixture;
using FluentAssertions;
using Moq;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.Infrastructure.JWT;
using PersonApi.V1.UseCase;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.UseCase
{
    [Collection("LogCall collection")]
    public class UpdatePersonByIdUseCaseTests
    {
        private readonly Mock<IPersonApiGateway> _mockGateway;
        private readonly ResponseFactory _responseFactory;
        private readonly Mock<ISnsGateway> _mockSnsGateway;
        private readonly Mock<ISnsFactory> _mockSnsFactory;
        private readonly UpdatePersonUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        public UpdatePersonByIdUseCaseTests()
        {
            _mockGateway = new Mock<IPersonApiGateway>();
            _responseFactory = new ResponseFactory(null);
            _mockSnsGateway = new Mock<ISnsGateway>();
            _mockSnsFactory = new Mock<ISnsFactory>();
            _classUnderTest = new UpdatePersonUseCase(_mockGateway.Object, _responseFactory,
                                                      _mockSnsGateway.Object, _mockSnsFactory.Object);
            _mockSnsGateway.Setup(x => x.Publish(It.IsAny<PersonSns>()));
        }

        private PersonQueryObject ConstructQuery(Guid id)
        {
            return new PersonQueryObject() { Id = id };
        }
        private UpdatePersonRequestObject ConstructRequest()
        {
            return new UpdatePersonRequestObject();
        }
        private Person ConstructPerson(Guid id)
        {
            return _fixture.Build<Person>()
                           .With(x => x.Id, id)
                           .Create();
        }

        [Fact]
        public async Task UpdatePersonByIdUseCaseGatewayReturnsFound()
        {
            // Arrange
            var request = ConstructRequest();
            var query = ConstructQuery(Guid.NewGuid());
            var token = new Token();
            var oldPerson = ConstructPerson(query.Id);
            var updatedPerson = ConstructPerson(query.Id);
            var gatewayResult = new UpdatePersonGatewayResult(oldPerson, updatedPerson);
            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(request, query)).ReturnsAsync(gatewayResult);
            var snsEvent = _fixture.Create<PersonSns>();
            _mockSnsFactory.Setup(x => x.Update(oldPerson, updatedPerson, token))
                           .Returns(snsEvent);

            // Act
            var response = await _classUnderTest.ExecuteAsync(request, token, query).ConfigureAwait(false);

            // Assert
            _mockSnsFactory.Verify(x => x.Update(oldPerson, updatedPerson, token), Times.Once);
            _mockSnsGateway.Verify(x => x.Publish(snsEvent), Times.Once);
            response.Should().BeEquivalentTo(_responseFactory.ToResponse(gatewayResult.UpdatedPerson));
        }

        [Fact]
        public void UpdatePersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var personRequest = new UpdatePersonRequestObject();
            var token = new Token();
            var query = ConstructQuery(Guid.NewGuid());
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(personRequest, query)).ThrowsAsync(exception);

            // Act
            Func<Task<PersonResponseObject>> func = async () => await _classUnderTest.ExecuteAsync(personRequest, token, query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
