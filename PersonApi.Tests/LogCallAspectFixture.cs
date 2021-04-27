using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Moq;
using PersonApi.V1.Logging;
using System;
using Xunit;

namespace PersonApi.Tests
{
    public class LogCallAspectFixture
    {
        public Mock<ILogger<LogCallAspect>> MockLogger { get; private set; }

        public LogCallAspectFixture()
        {
            MockLogger = SetupLogCallAspect();
        }

        private static Mock<ILogger<LogCallAspect>> SetupLogCallAspect()
        {
            var mockLogger = new Mock<ILogger<LogCallAspect>>();
            var mockAspect = new Mock<LogCallAspect>(mockLogger.Object);
            var mockAppServices = new Mock<IServiceProvider>();
            var appBuilder = new Mock<IApplicationBuilder>();

            appBuilder.SetupGet(x => x.ApplicationServices).Returns(mockAppServices.Object);
            LogCallAspectServices.UseLogCall(appBuilder.Object);
            mockAppServices.Setup(x => x.GetService(typeof(LogCallAspect))).Returns(mockAspect.Object);
            return mockLogger;
        }
    }

    [CollectionDefinition("LogCall collection")]
    public class LogCallAspectFixtureCollection : ICollectionFixture<LogCallAspectFixture>
    { }
}
