using AutoFixture;
using FluentAssertions;
using Hackney.Core.Http;
using Hackney.Core.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Controllers;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using PersonApi.V1.Infrastructure.Exceptions;
using PersonApi.V1.UseCase.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
        private readonly ResponseFactory _responseFactory;
        private readonly HeaderDictionary _requestHeaders;
        private readonly Mock<HttpResponse> _mockHttpResponse;
        private readonly HeaderDictionary _responseHeaders;

        private const string RequestBodyText = "Some request body text";
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
            _mockHttpResponse = new Mock<HttpResponse>();

            _responseFactory = new ResponseFactory(null);

            _sut = new PersonApiController(_mockGetByIdUseCase.Object, _mockNewPersonUseCase.Object, _mockUpdatePersonUseCase.Object,
                _mockTokenFactory.Object, _mockContextWrapper.Object, _responseFactory);

            _mockHttpRequest.SetupGet(x => x.Body).Returns(new MemoryStream(Encoding.Default.GetBytes(RequestBodyText)));
            _requestHeaders = new HeaderDictionary();
            _mockHttpRequest.SetupGet(x => x.Headers).Returns(_requestHeaders);
            _mockContextWrapper.Setup(x => x.GetContextRequestHeaders(It.IsAny<HttpContext>())).Returns(_requestHeaders);
            _responseHeaders = new HeaderDictionary();
            _mockHttpResponse.SetupGet(x => x.Headers).Returns(_responseHeaders);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(x => x.Request).Returns(_mockHttpRequest.Object);
            mockHttpContext.SetupGet(x => x.Response).Returns(_mockHttpResponse.Object);

            var controllerContext = new ControllerContext(new ActionContext(mockHttpContext.Object, new RouteData(), new ControllerActionDescriptor()));
            _sut.ControllerContext = controllerContext;
        }

        private PersonQueryObject ConstructQuery()
        {
            return new PersonQueryObject() { Id = Guid.NewGuid() };
        }

        private UpdatePersonRequestObject ConstructRequest()
        {
            return new UpdatePersonRequestObject();
        }

        [Fact]
        public async Task GetPersonByIdAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var query = ConstructQuery();
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(query)).ReturnsAsync((Person) null);

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
            var personResponse = _fixture.Create<Person>();
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(query)).ReturnsAsync(personResponse);

            // Act
            var response = await _sut.GetPersonByIdAsync(query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            _sut.HttpContext.Response.Headers.TryGetValue(HeaderConstants.ETag, out StringValues val).Should().BeTrue();
            val.First().Should().Be(personResponse.VersionNumber.ToString());
            (response as OkObjectResult).Value.Should().BeEquivalentTo(_responseFactory.ToResponse(personResponse));
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
            _mockNewPersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<CreatePersonRequestObject>(), It.IsAny<Token>()))
                                 .ThrowsAsync(exception);

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
            _mockUpdatePersonUseCase.Setup(x => x.ExecuteAsync(request, RequestBodyText, It.IsAny<Token>(), query, It.IsAny<int?>()))
                                    .ReturnsAsync((PersonResponseObject) null);

            // Act
            var response = await _sut.UpdatePersonByIdAsync(request, query).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(query.Id);
        }

        [Fact]
        public async Task UpdatePersonByIdAsyncFoundReturnsFound()
        {
            // Arrange
            var query = ConstructQuery();
            var request = ConstructRequest();
            var personResponse = _fixture.Create<PersonResponseObject>();
            _mockUpdatePersonUseCase.Setup(x => x.ExecuteAsync(request, RequestBodyText, It.IsAny<Token>(), query, It.IsAny<int?>()))
                                    .ReturnsAsync(personResponse);

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
            _mockUpdatePersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<UpdatePersonRequestObject>(), RequestBodyText, It.IsAny<Token>(), query, It.IsAny<int?>()))
                                    .ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _sut.UpdatePersonByIdAsync(new UpdatePersonRequestObject(), query)
                .ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData(0, 1)]
        [InlineData(0, null)]
        [InlineData(2, 1)]
        public async Task UpdatePersonByIdAsyncVersionNumberConflictExceptionReturns409(int? expected, int? actual)
        {
            // Arrange
            var query = ConstructQuery();
            _requestHeaders.Add(HeaderConstants.IfMatch, new StringValues(expected?.ToString()));

            var exception = new VersionNumberConflictException(expected, actual);
            _mockUpdatePersonUseCase.Setup(x => x.ExecuteAsync(It.IsAny<UpdatePersonRequestObject>(), RequestBodyText, It.IsAny<Token>(), query, expected))
                                    .ThrowsAsync(exception);

            // Act
            var result = await _sut.UpdatePersonByIdAsync(new UpdatePersonRequestObject(), query).ConfigureAwait(false);

            // Assert
            result.Should().BeOfType(typeof(ConflictObjectResult));
            (result as ConflictObjectResult).Value.Should().BeEquivalentTo(exception.Message);
        }
    }
}
