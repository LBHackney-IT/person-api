using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Controllers;
using PersonApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Infrastructure.JWT;
using Xunit;

namespace PersonApi.Tests.V1.Controllers
{
    [Collection("LogCall collection")]
    public class PersonApiControllerTests
    {
        private readonly Mock<IGetByIdUseCase> _mockGetByIdUseCase;
        private readonly Mock<IPostNewPersonUseCase> _mockNewPersonUseCase;
        private readonly Mock<ITokenFactory> _mockTokenFactory;

        private readonly PersonApiController _sut;
        private readonly Fixture _fixture = new Fixture();

        public PersonApiControllerTests()
        {
            _mockGetByIdUseCase = new Mock<IGetByIdUseCase>();
            _mockNewPersonUseCase = new Mock<IPostNewPersonUseCase>();
            _mockTokenFactory = new Mock<ITokenFactory>();

            _sut = new PersonApiController(_mockGetByIdUseCase.Object, _mockNewPersonUseCase.Object, _mockTokenFactory.Object);
        }

        [Fact]
        public async Task GetPersonByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(id)).ReturnsAsync((PersonResponseObject) null);

            // Act
            var response = await _sut.GetPersonByIdAsync(id).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Fact]
        public async Task GetPersonByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var personResponse = _fixture.Create<PersonResponseObject>();
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(id)).ReturnsAsync(personResponse);

            // Act
            var response = await _sut.GetPersonByIdAsync(id).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult).Value.Should().Be(personResponse);
        }

        [Fact]
        public void GetPersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(id)).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _sut.GetPersonByIdAsync(id).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        [Fact]
        public async Task PostNewPersonIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var personResponse = _fixture.Create<PersonResponseObject>();
            _mockNewPersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PersonRequestObject>(), It.IsAny<Token>()))
                .ReturnsAsync(personResponse);

            // Act
            var response = await _sut.PostNewPerson(new PersonRequestObject()).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(CreatedResult));
            (response as CreatedResult).Value.Should().Be(personResponse);
        }

        [Fact]
        public void PostNewPersonIdAsyncExceptionIsThrown()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _mockNewPersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PersonRequestObject>(), It.IsAny<Token>())).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _sut.PostNewPerson(new PersonRequestObject())
                .ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
