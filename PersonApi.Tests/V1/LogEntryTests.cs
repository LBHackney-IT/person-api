using FluentAssertions;
using Microsoft.Extensions.Logging;
using PersonApi.V1;
using System;
using Xunit;

namespace PersonApi.Tests.V1
{
    public class LogEntryTests
    {
        [Fact]
        public void LogEntryTestDefaultConstructor()
        {
            var sut = new LogEntry();
            sut.CorrelationId.Should().BeNull();
            sut.Timestamp.Should().Be(default(DateTime));
            sut.UserId.Should().BeNull();
            sut.Level.Should().Be(default(LogLevel));
            sut.Message.Should().BeNull();
            sut.ExceptionObject.Should().BeNull();
            sut.Exception.Should().BeNull();
        }

        [Fact]
        public void LogEntryTestHasExceptionObject()
        {
            var ex = new Exception("Some exception");
            var sut = new LogEntry
            {
                ExceptionObject = ex
            };
            sut.Exception.Should().Be(ex.ToString());
        }
    }
}
