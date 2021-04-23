using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using PersonApi.V1;
using PersonApi.V1.Context;
using PersonApi.V1.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1
{
    public class CorrelationMiddlewareTest
    {
        private readonly CorrelationMiddleware _sut;

        public CorrelationMiddlewareTest()
        {
            _sut = new CorrelationMiddleware(null);
        }

        [Fact]
        public async Task DoesNotReplaceCorrelationIdIfOneExists()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var headerValue = Guid.NewGuid().ToString();

            httpContext.HttpContext.Request.Headers.Add(Constants.CorrelationId, new StringValues(headerValue));

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            httpContext.HttpContext.Request.Headers[Constants.CorrelationId].Should().BeEquivalentTo(headerValue);
        }

        [Fact]
        public async Task AddsCorrelationIdIfOneDoesNotExist()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            httpContext.HttpContext.Request.Headers[Constants.CorrelationId].Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task UpdatesContext()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var correlationId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();

            httpContext.HttpContext.Request.Headers.Add(Constants.CorrelationId, new StringValues(correlationId.ToString()));
            httpContext.HttpContext.Request.Headers.Add(Constants.UserId, new StringValues(userId));

            ICurrentContext ctx = null;
            CallContext.OnCurrentChanged += (sender, e) => { ctx = e; };

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            ctx.Should().NotBeNull();
            ctx.CorrelationId.Should().Be(correlationId);
            ctx.UserId.Should().Be(userId);
        }
    }
}
