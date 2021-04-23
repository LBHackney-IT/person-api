using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PersonApi.V1.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.Logging
{
    public class MethodLoggerTests
    {
        private const string Method = "Some Method";
        private bool _methodCalled = false;

        private readonly Action _voidAction;
        private readonly Func<bool> _resultAction;
        private readonly Func<Task> _taskAction;
        private readonly Func<Task<bool>> _taskResultAction;

        private readonly Mock<IApiLogger> _mockLogger = new Mock<IApiLogger>();
        private readonly MethodLogger _sut;

        public MethodLoggerTests()
        {
            _sut = new MethodLogger(_mockLogger.Object);

            _voidAction = () => _methodCalled = true;
            _taskAction = async () =>
            {
                await Task.Run(() => _methodCalled = true).ConfigureAwait(false);
            };
            _resultAction = () =>
            {
                _methodCalled = true;
                return _methodCalled;
            };
            _taskResultAction = async () =>
            {
                return await Task.Run(() =>
                {
                    _methodCalled = true;
                    return _methodCalled;
                }).ConfigureAwait(false);
            };
        }

        private void VerifyLogging(string method)
        {
            _mockLogger.Verify(x => x.Log(LogLevel.Information,
                It.Is<string>(y => y.Contains($"{method} STARTING")), null), Times.Once);
            _mockLogger.Verify(x => x.Log(LogLevel.Information,
                It.Is<string>(y => y.Contains($"{method} ENDING")), null), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ExecuteVoidTestInvalidMethodDescThrows(string desc)
        {
            Action act = () => _sut.Execute(desc, _voidAction);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ExecuteVoidTestNullActionThrows()
        {
            Action act = () => _sut.Execute(Method, (Action) null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ExecuteVoidTestLogsCorrectly()
        {
            _sut.Execute(Method, _voidAction);
            _methodCalled.Should().BeTrue();

            VerifyLogging(Method);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ExecuteWithResultTestInvalidMethodDescThrows(string desc)
        {
            Action act = () => _sut.Execute(desc, _resultAction);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ExecuteWithResultTestNullActionThrows()
        {
            Action act = () => _sut.Execute(Method, (Func<bool>) null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ExecuteWithResultTestLogsCorrectly()
        {
            var result = _sut.Execute(Method, _resultAction);
            result.Should().BeTrue();
            _methodCalled.Should().BeTrue();

            VerifyLogging(Method);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ExecuteAsyncTaskTestInvalidMethodDescThrows(string desc)
        {
            Func<Task> act =
                async () => await _sut.ExecuteAsync(desc, _taskAction).ConfigureAwait(false);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ExecuteAsyncTaskTestNullActionThrows()
        {
            Func<Task> act =
                async () => await _sut.ExecuteAsync(Method, (Func<Task>) null).ConfigureAwait(false);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task ExecuteAsyncTaskTestLogsCorrectly()
        {
            await _sut.ExecuteAsync(Method, _taskAction).ConfigureAwait(false);
            _methodCalled.Should().BeTrue();

            VerifyLogging(Method);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ExecuteAsyncWithResultTestInvalidMethodDescThrows(string desc)
        {
            Func<Task> act =
                async () => await _sut.ExecuteAsync(desc, _taskResultAction).ConfigureAwait(false);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ExecuteAsyncWithResultTestNullActionThrows()
        {
            Func<Task> act =
                async () => await _sut.ExecuteAsync(Method, (Func<Task<bool>>) null).ConfigureAwait(false);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task ExecuteAsyncWithResultTestLogsCorrectly()
        {
            var result = await _sut.ExecuteAsync(Method, _taskResultAction).ConfigureAwait(false);
            result.Should().BeTrue();
            _methodCalled.Should().BeTrue();

            VerifyLogging(Method);
        }
    }
}
