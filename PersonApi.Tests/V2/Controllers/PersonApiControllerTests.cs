using AutoFixture;
using FluentAssertions;
using Hackney.Core.Http;
using Hackney.Core.JWT;
using Hackney.Shared.Person.Boundary.Request;
using Hackney.Shared.Person.Boundary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonApi.V2.Controllers;
using PersonApi.V2.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V2.Controllers
{
    [Collection("LogCall collection")]
    public class PersonApiControllerTests
    {
        private readonly Mock<IPostNewPersonUseCase> _mockNewPersonUseCase;
        private readonly Mock<ITokenFactory> _mockTokenFactory;
        private readonly Mock<IHttpContextWrapper> _mockContextWrapper;

        private readonly PersonApiController _sut;
        private readonly Fixture _fixture = new Fixture();

        public PersonApiControllerTests()
        {
            _mockNewPersonUseCase = new Mock<IPostNewPersonUseCase>();
            _mockTokenFactory = new Mock<ITokenFactory>();
            _mockContextWrapper = new Mock<IHttpContextWrapper>();

            _sut = new PersonApiController(_mockNewPersonUseCase.Object,
                _mockTokenFactory.Object, _mockContextWrapper.Object);

            _mockContextWrapper.Setup(x => x.GetContextRequestHeaders(It.IsAny<HttpContext>())).Returns(new HeaderDictionary());
            _mockTokenFactory.Setup(x => x.Create(It.IsAny<HeaderDictionary>(), It.IsAny<string>())).Returns(new Token());
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
    }
}
