using AutoFixture;
using FluentAssertions;
using Hackney.Core.JWT;
using Moq;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using Hackney.Shared.Person;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.Infrastructure;
using PersonApi.V1.UseCase;
using System;
using System.Collections.Generic;
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

        [Theory]
        [InlineData(null)]
        [InlineData(3)]
        public async Task UpdatePersonByIdUseCaseGatewayReturnsResult(int? ifMatch)
        {
            // Arrange
            var request = ConstructRequest();
            var query = ConstructQuery(Guid.NewGuid());
            var token = new Token();
            var updatedPerson = ConstructPerson(query.Id);
            var gatewayResult = new UpdateEntityResult<PersonDbEntity>()
            {
                UpdatedEntity = updatedPerson.ToDatabase(),
                OldValues = new Dictionary<string, object>
                {
                    {  "firstName", "old first name" },
                    {  "surname", "old surname" }
                },
                NewValues = new Dictionary<string, object>
                {
                    {  "firstName", "New first name" },
                    {  "surname", "New surname" }
                }
            };
            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(request, It.IsAny<string>(), query, ifMatch))
                        .ReturnsAsync(gatewayResult);
            var snsEvent = _fixture.Create<PersonSns>();
            _mockSnsFactory.Setup(x => x.Update(gatewayResult, token))
                           .Returns(snsEvent);

            // Act
            var response = await _classUnderTest.ExecuteAsync(request, "", token, query, ifMatch).ConfigureAwait(false);

            // Assert
            _mockSnsFactory.Verify(x => x.Update(gatewayResult, token), Times.Once);
            _mockSnsGateway.Verify(x => x.Publish(snsEvent), Times.Once);
            response.Should().BeEquivalentTo(_responseFactory.ToResponse(updatedPerson));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(3)]
        public async Task UpdatePersonByIdUseCaseGatewayReturnsResultNoChanges(int? ifMatch)
        {
            // Arrange
            var request = ConstructRequest();
            var query = ConstructQuery(Guid.NewGuid());
            var token = new Token();
            var updatedPerson = ConstructPerson(query.Id);
            var gatewayResult = new UpdateEntityResult<PersonDbEntity>()
            {
                UpdatedEntity = updatedPerson.ToDatabase()
            };
            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(request, It.IsAny<string>(), query, ifMatch))
                        .ReturnsAsync(gatewayResult);
            var snsEvent = _fixture.Create<PersonSns>();
            _mockSnsFactory.Setup(x => x.Update(gatewayResult, token))
                           .Returns(snsEvent);

            // Act
            var response = await _classUnderTest.ExecuteAsync(request, "", token, query, ifMatch).ConfigureAwait(false);

            // Assert
            _mockSnsFactory.Verify(x => x.Update(gatewayResult, token), Times.Never);
            _mockSnsGateway.Verify(x => x.Publish(snsEvent), Times.Never);
            response.Should().BeEquivalentTo(_responseFactory.ToResponse(updatedPerson));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(3)]
        public async Task UpdatePersonByIdUseCaseGatewayReturnsNull(int? ifMatch)
        {
            // Arrange
            var request = ConstructRequest();
            var query = ConstructQuery(Guid.NewGuid());
            var token = new Token();

            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(request, It.IsAny<string>(), query, ifMatch))
                        .ReturnsAsync((UpdateEntityResult<PersonDbEntity>) null);

            // Act
            var response = await _classUnderTest.ExecuteAsync(request, "", token, query, ifMatch).ConfigureAwait(false);

            // Assert
            _mockSnsFactory.Verify(x => x.Update(It.IsAny<UpdateEntityResult<PersonDbEntity>>(), It.IsAny<Token>()), Times.Never);
            _mockSnsGateway.Verify(x => x.Publish(It.IsAny<PersonSns>()), Times.Never);
            response.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(3)]
        public void UpdatePersonByIdAsyncExceptionIsThrown(int? ifMatch)
        {
            // Arrange
            var personRequest = new UpdatePersonRequestObject();
            var token = new Token();
            var query = ConstructQuery(Guid.NewGuid());
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(personRequest, It.IsAny<string>(), query, ifMatch)).ThrowsAsync(exception);

            // Act
            Func<Task<PersonResponseObject>> func = async () =>
                await _classUnderTest.ExecuteAsync(personRequest, "", token, query, ifMatch).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
