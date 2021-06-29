using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Controllers;
using PersonApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PersonApi.V1.Infrastructure;
using PersonApi.V1.Infrastructure.JWT;
using Xunit;
using Bogus;

namespace PersonApi.Tests.V1.Controllers
{
    [Collection("LogCall collection")]
    public class PersonApiControllerTests
    {
        private readonly Mock<IGetByIdUseCase> _mockGetByIdUseCase;
        private readonly Mock<IPostNewPersonUseCase> _mockNewPersonUseCase;
        private readonly Mock<IUpdatePersonUseCase> _mockUpdatePersonUseCase;
        private readonly Mock<ITokenFactory> _mockTokenFactory;
        private readonly Mock<IHttpContextWrapper> _mockContextWrapper;
        private readonly Mock<HttpRequest> _mockHttpRequest;

        private readonly PersonApiController _sut;
        private readonly Fixture _fixture = new Fixture();

        public PersonApiControllerTests()
        {
            _mockGetByIdUseCase = new Mock<IGetByIdUseCase>();
            _mockNewPersonUseCase = new Mock<IPostNewPersonUseCase>();
            _mockUpdatePersonUseCase = new Mock<IUpdatePersonUseCase>();
            _mockTokenFactory = new Mock<ITokenFactory>();
            _mockContextWrapper = new Mock<IHttpContextWrapper>();
            _mockHttpRequest = new Mock<HttpRequest>();

            _sut = new PersonApiController(_mockGetByIdUseCase.Object, _mockNewPersonUseCase.Object, _mockUpdatePersonUseCase.Object,
                _mockTokenFactory.Object, _mockContextWrapper.Object);

            _mockContextWrapper.Setup(x => x.GetContextRequestHeaders(It.IsAny<HttpContext>())).Returns(new HeaderDictionary());
        }

        private PersonQueryObject ConstructQuery()
        {
            return new PersonQueryObject() { Id = Guid.NewGuid() };
        }

        private UpdatePersonRequestObject ConstructRequest()
        {
            return new UpdatePersonRequestObject() { Id = Guid.NewGuid() };
        }

        [Fact]
        public async Task GetPersonByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(query)).ReturnsAsync((PersonResponseObject) null);

            // Act
            var response = await _sut.GetPersonByIdAsync(query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(query.Id);
        }

        [Fact]
        public async Task GetPersonByIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var query = ConstructQuery();
            var personResponse = _fixture.Create<PersonResponseObject>();
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(query)).ReturnsAsync(personResponse);

            // Act
            var response = await _sut.GetPersonByIdAsync(query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult).Value.Should().Be(personResponse);
        }

        [Fact]
        public void GetPersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var query = ConstructQuery();
            var exception = new ApplicationException("Test exception");
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(query)).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _sut.GetPersonByIdAsync(query).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        [Fact]
        public async Task PostNewPersonIdAsyncFoundReturnsResponse()
        {
            // Arrange
            var personResponse = _fixture.Create<PersonResponseObject>();
            _mockNewPersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<CreatePersonRequestObject>(), It.IsAny<Token>()))
                .ReturnsAsync(personResponse);

            // Act
            var response = await _sut.PostNewPerson(new CreatePersonRequestObject()).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(CreatedResult));
            (response as CreatedResult).Value.Should().Be(personResponse);
        }

        [Fact]
        public void PostNewPersonIdAsyncExceptionIsThrown()
        {
            // Arrange
            var exception = new ApplicationException("Test exception");
            _mockNewPersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<CreatePersonRequestObject>(), It.IsAny<Token>())).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _sut.PostNewPerson(new CreatePersonRequestObject())
                .ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
        [Fact]
        public async Task UpdatePersonByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var query = ConstructQuery();
            var request = ConstructRequest();
            _mockUpdatePersonUseCase.Setup(x => x.ExecuteAsync(request, It.IsAny<Token>())).ReturnsAsync((PersonResponseObject) null);

            // Act
            var response = await _sut.UpdatePersonByIdAsync(request, query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(request.Id);
        }

        [Fact]
        public async Task UpdatePersonByIdAsyncFoundReturnsFound()
        {
            // Arrange
            var query = ConstructQuery();
            var request = ConstructRequest();
            var personResponse = _fixture.Create<PersonResponseObject>();
            _mockUpdatePersonUseCase.Setup(x => x.ExecuteAsync(request, It.IsAny<Token>())).ReturnsAsync(personResponse);

            // Act
            var response = await _sut.UpdatePersonByIdAsync(request, query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NoContentResult));
        }

        [Fact]
        public void UpdatePersonByIdAsyncExceptionIsThrown()
        {
            // Arrange
            var query = ConstructQuery();
            var exception = new ApplicationException("Test exception");
            _mockUpdatePersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<UpdatePersonRequestObject>(), It.IsAny<Token>())).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _sut.UpdatePersonByIdAsync(new UpdatePersonRequestObject(), query)
                .ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
