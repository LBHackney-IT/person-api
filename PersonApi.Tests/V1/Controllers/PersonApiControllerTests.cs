using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Controllers;
using PersonApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;

namespace PersonApi.Tests.V1.Controllers
{
    [TestFixture]
    public class PersonApiControllerTests
    {
        private Mock<IGetByIdUseCase> _mockGetByIdUseCase;
        private PersonApiController _sut;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _mockGetByIdUseCase = new Mock<IGetByIdUseCase>();
            _sut = new PersonApiController(_mockGetByIdUseCase.Object);
        }

        [Test]
        public async Task GetPersonByIdAsyncNotFoundReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(id)).ReturnsAsync((PersonResponseObject) null);
            var response = await _sut.GetPersonByIdAsync(id).ConfigureAwait(false);

            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult).Value.Should().Be(id);
        }

        [Test]
        public async Task GetPersonByIdAsyncFoundReturnsResponse()
        {
            var id = Guid.NewGuid();
            var personResponse = _fixture.Create<PersonResponseObject>();
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(id)).ReturnsAsync(personResponse);
            var response = await _sut.GetPersonByIdAsync(id).ConfigureAwait(false);

            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult).Value.Should().Be(personResponse);
        }

        [Test]
        public void GetPersonByIdAsyncExceptionIsThrown()
        {
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _mockGetByIdUseCase.Setup(x => x.ExecuteAsync(id)).ThrowsAsync(exception);

            Func<Task<IActionResult>> func = async () => await _sut.GetPersonByIdAsync(id).ConfigureAwait(false);

            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
