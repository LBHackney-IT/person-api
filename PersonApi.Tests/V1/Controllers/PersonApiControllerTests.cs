using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonApi.V1;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Controllers;
using PersonApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.Controllers
{
    public class PersonApiControllerTests
    {
        private readonly MethodLogger _methodLogger;
        private readonly Mock<IApiLogger> _mockLogger = new Mock<IApiLogger>();
        private readonly Mock<IGetByIdUseCase> _mockGetByIdUseCase;
        private readonly PersonApiController _sut;
        private readonly Fixture _fixture = new Fixture();

        public PersonApiControllerTests()
        {
            _methodLogger = new MethodLogger(_mockLogger.Object);
            _mockGetByIdUseCase = new Mock<IGetByIdUseCase>();
            _sut = new PersonApiController(_methodLogger, _mockGetByIdUseCase.Object);
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
    }
}
