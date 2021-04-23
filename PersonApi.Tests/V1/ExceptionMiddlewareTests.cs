using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using PersonApi.V1;
using PersonApi.V1.Controllers;
using PersonApi.V1.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1
{
    public class ExceptionMiddlewareTests
    {
        private readonly string _traceId = Guid.NewGuid().ToString();
        private readonly string _correlationId = Guid.NewGuid().ToString();
        private const int DEFAULTERRORCODE = 500;
        private const string DEFAULTERRORMESSAGE = "Internal Server Error.";

        private readonly HttpContext _httpContext;
        private readonly Mock<IExceptionHandlerFeature> _mockExHandlerFeature;

        private readonly Mock<IApiLogger> _mockLogger;

        public ExceptionMiddlewareTests()
        {
            _httpContext = new DefaultHttpContext();
            _httpContext.TraceIdentifier = _traceId;
            _httpContext.Request.Headers.Add(Constants.CorrelationId, new StringValues(_correlationId));
            _httpContext.Response.Body = new MemoryStream();
            _httpContext.Response.StatusCode = DEFAULTERRORCODE;

            _mockExHandlerFeature = new Mock<IExceptionHandlerFeature>();
            _httpContext.Features.Set(_mockExHandlerFeature.Object);

            _mockLogger = new Mock<IApiLogger>();
        }

        private async Task VerifyResponse(string resultMessage = DEFAULTERRORMESSAGE, int statusCode = DEFAULTERRORCODE)
        {
            _httpContext.Response.ContentType.Should().Be("application/json");

            var expected = new ExceptionResult(resultMessage, _traceId, _correlationId, statusCode).ToString();
            _httpContext.Response.Body.Position = 0;
            using (StreamReader streamReader = new StreamReader(_httpContext.Response.Body))
            {
                string actual = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                expected.Should().Be(actual);
            }
        }

        [Fact]
        public async Task HandleExceptionsTestNoExceptionHandlerWritesResponse()
        {
            // Arrange
            _httpContext.Features.Set<IExceptionHandlerFeature>(null);

            // Act
            await ExceptionMiddlewareExtensions.HandleExceptions(_httpContext, _mockLogger.Object)
                                               .ConfigureAwait(false);

            // Assert
            await VerifyResponse().ConfigureAwait(false);
        }

        [Fact]
        public async Task HandleExceptionsTestWithHandlerButNoExceptionWritesResponse()
        {
            // Act
            await ExceptionMiddlewareExtensions.HandleExceptions(_httpContext, _mockLogger.Object)
                                               .ConfigureAwait(false);

            // Assert
            await VerifyResponse().ConfigureAwait(false);
            _mockLogger.Verify(x => x.Log(LogLevel.Error,
                It.Is<string>(y => y.Contains("Exception thrown")), null), Times.Once);
        }

        [Fact]
        public async Task HandleExceptionsTestWithHandlerWithExceptionWritesResponse()
        {
            // Arrange
            var exMessage = "This is an exception";
            var exception = new Exception(exMessage);
            _mockExHandlerFeature.SetupGet(x => x.Error).Returns(exception);

            // Act
            await ExceptionMiddlewareExtensions.HandleExceptions(_httpContext, _mockLogger.Object)
                                               .ConfigureAwait(false);

            // Assert
            await VerifyResponse(exMessage).ConfigureAwait(false);
            _mockLogger.Verify(x => x.Log(LogLevel.Error,
                It.Is<string>(y => y.Contains("Exception thrown")), exception), Times.Once);
        }
    }
}
