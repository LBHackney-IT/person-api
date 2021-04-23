using FluentAssertions;
using PersonApi.V1.Context;
using System;
using PersonApi.V1.Controllers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using PersonApi.V1.Logging;
using Microsoft.Extensions.Logging;

namespace PersonApi.Tests.V1.Logging
{
    public class ApiLoggerTests
    {
        private readonly StringWriter _consoleOut;
        private readonly ApiLogger _sut;

        public ApiLoggerTests()
        {
            _sut = new ApiLogger();

            _consoleOut = new StringWriter();
            Console.SetOut(_consoleOut);
        }

        [Fact]
        public void ApiLoggerTestLogsMessage()
        {
            var message = "Some log message";

            _sut.Log(LogLevel.Information, message);

            string console = _consoleOut.GetStringBuilder().ToString();
            console.Should().Contain(Enum.GetName(typeof(LogLevel), LogLevel.Information));
            console.Should().Contain(message);

        }

        [Fact]
        public void ApiLoggerTestLogsMessageWithException()
        {
            var message = "Some log message";
            var exMessage = "An EXCEPTION";
            var exception = new ApplicationException(exMessage);

            _sut.Log(LogLevel.Information, message, exception);

            string console = _consoleOut.GetStringBuilder().ToString();
            console.Should().Contain(Enum.GetName(typeof(LogLevel), LogLevel.Information));
            console.Should().Contain(message);
            console.Should().Contain(exMessage);
        }

        [Fact]
        public void ApiLoggerTestLogsMessageWithContextInfo()
        {
            var message = "Some log message";
            var correlationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            CallContext.NewContext();
            CallContext.Current.SetValue(Constants.CorrelationId, correlationId);
            CallContext.Current.SetValue(Constants.UserId, userId);

            _sut.Log(LogLevel.Information, message);

            string console = _consoleOut.GetStringBuilder().ToString();
            console.Should().Contain(Enum.GetName(typeof(LogLevel), LogLevel.Information));
            console.Should().Contain(message);
            console.Should().Contain(correlationId.ToString());
            console.Should().Contain(userId.ToString());
        }
    }
}
