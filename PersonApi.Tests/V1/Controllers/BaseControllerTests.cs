using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using PersonApi.V1.Controllers;
using Moq;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;
using System.Collections.Generic;

namespace PersonApi.Tests.V1.Controllers
{
    [TestFixture]
    public class BaseControllerTests
    {
        private BaseController _classUnderTest;
        private ControllerContext _controllerContext;
        private HttpContext _httpContext;

        [SetUp]
        public void Setup()
        {
            _httpContext = new DefaultHttpContext();
            _classUnderTest = new BaseController();
            _controllerContext = new ControllerContext(new ActionContext(_httpContext, new RouteData(), new ControllerActionDescriptor()));
            _classUnderTest.ControllerContext = _controllerContext;
        }

        [Test]
        public void ShouldThrowExceptionIfNoCorrelationHeader()
        {
            _classUnderTest.Invoking(x => x.GetCorrelationId()).Should().Throw<KeyNotFoundException>().WithMessage("Request is missing a correlationId");
        }

        [Test]
        public void GetCorrelationShouldReturnCorrelationIdWhenExists()
        {
            // Arrange
            _httpContext.Request.Headers.Add(CorrelationConstants.CorrelationId, "123");

            // Act
            var response = _classUnderTest.GetCorrelationId();

            // Assert
            response.Should().BeEquivalentTo("123");
        }


    }
}
