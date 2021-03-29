using FluentAssertions;
using NUnit.Framework;
using PersonApi.V1;
using System;

namespace PersonApi.Tests.V1
{
    [TestFixture]
    public class ExceptionResultTests
    {
        private readonly string _errorMessage = "This is some error message";
        private readonly string _traceId = Guid.NewGuid().ToString();
        private readonly string _correlationId = Guid.NewGuid().ToString();

        [TestCase]
        public void ExceptionResultTestConstructorWithNoStatusCode()
        {
            var sut = new ExceptionResult(_errorMessage, _traceId, _correlationId);
            sut.CorrelationId.Should().Be(_correlationId);
            sut.Message.Should().Be(_errorMessage);
            sut.StatusCode.Should().Be(ExceptionResult.DefaultStatusCode);
            sut.TraceId.Should().Be(_traceId);
        }

        [TestCase(200)]
        [TestCase(404)]
        public void ExceptionResultTestConstructorWithStatusCode(int statusCode)
        {
            var sut = new ExceptionResult(_errorMessage, _traceId, _correlationId, statusCode);
            sut.CorrelationId.Should().Be(_correlationId);
            sut.Message.Should().Be(_errorMessage);
            sut.StatusCode.Should().Be(statusCode);
            sut.TraceId.Should().Be(_traceId);
        }
    }
}
