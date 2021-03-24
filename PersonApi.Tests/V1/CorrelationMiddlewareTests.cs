using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi.Tests.V1.Infrastructure
{
    [TestFixture]
    public class CorrelationMiddlewareTests
    {
        private CorrelationMiddleware _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _classUnderTest = new CorrelationMiddleware(null);
        }

        [Test]
        public async Task AddCorrelationIdIfItDoesNotExist()
        {
            var httpContext = new DefaultHttpContext();
            await _classUnderTest.InvokeAsync(httpContext).ConfigureAwait(false);
            httpContext.HttpContext.Request.Headers[CorrelationConstants.CorrelationId].Should().HaveCountGreaterThan(0);
        }

        [Test]
        public async Task DoesNotAddNewCorrelationIdIfAlreadyExist()
        {
            var httpContext = new DefaultHttpContext();
            var correlationIdValue = "11234";
            httpContext.HttpContext.Request.Headers.Add(CorrelationConstants.CorrelationId, correlationIdValue);
            await _classUnderTest.InvokeAsync(httpContext).ConfigureAwait(false);
            httpContext.HttpContext.Request.Headers[CorrelationConstants.CorrelationId].Should().BeEquivalentTo(correlationIdValue);


        }
    }
}
