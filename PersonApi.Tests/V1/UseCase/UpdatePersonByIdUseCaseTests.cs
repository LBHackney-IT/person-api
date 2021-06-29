using AutoFixture;
using FluentAssertions;
using Moq;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.UseCase
{
    [Collection("LogCall collection")]
    public class UpdatePersonByIdUseCaseTests
    {
        private readonly Mock<IPersonApiGateway> _mockGateway;
        private readonly ResponseFactory _responseFactory;
        private readonly UpdatePersonUseCase _classUnderTest;
        private readonly Fixture _fixture = new Fixture();

        public UpdatePersonByIdUseCaseTests()
        {
            _mockGateway = new Mock<IPersonApiGateway>();
            _responseFactory = new ResponseFactory(null);
            _classUnderTest = new UpdatePersonUseCase(_mockGateway.Object, _responseFactory);
        }

        private UpdatePersonRequestObject ConstructRequest()
        {
            return new UpdatePersonRequestObject() { Id = Guid.NewGuid() };
        }

        [Fact]
        public async Task UpdatePersonByIdUseCaseGatewayReturnsNullReturnsNull()
        {
            // Arrange
            var request = ConstructRequest();
            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(request)).ReturnsAsync((Person) null);

            // Act
            var response = await _classUnderTest.ExecuteAsync(request).ConfigureAwait(false);

            // Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task UpdatePersonByIdUseCaseGatewayReturnsFound()
        {
            // Arrange
            var request = ConstructRequest();
            var person = _fixture.Create<Person>();
            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(request)).ReturnsAsync(person);

            // Act
            var response = await _classUnderTest.ExecuteAsync(request).ConfigureAwait(false);

            // Assert
            response.Should().BeEquivalentTo(_responseFactory.ToResponse(person));
        }

        [Fact]
        public void UpdatePersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var personRequest = new UpdatePersonRequestObject();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.UpdatePersonByIdAsync(personRequest)).ThrowsAsync(exception);

            // Act
            Func<Task<PersonResponseObject>> func = async () => await _classUnderTest.ExecuteAsync(personRequest).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
