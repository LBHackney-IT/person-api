using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PersonApi.V1.Logging;
using System;
using System.Linq;
using Xunit;

namespace PersonApi.Tests.V1.Logging
{
    public class LoggingExtensionsTests
    {
        private static bool IsServiceRegistered<TInterface, TImpl>(ServiceDescriptor service)
        {
            return (service.ServiceType == typeof(TInterface))
                && (service.ImplementationType == typeof(TImpl));
        }

        [Fact]
        public void RegisterLoggingTestNullServicesThrows()
        {
            IServiceCollection services = null;
            Action act = () => services.AddLogging();
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RegisterLoggingTestRegistersLoggingClasses()
        {
            IServiceCollection services = new ServiceCollection();
            services.RegisterLogging();
            services.Any(x => IsServiceRegistered<IApiLogger, ApiLogger>(x)).Should().BeTrue();
            services.Any(x => IsServiceRegistered<IMethodLogger, MethodLogger>(x)).Should().BeTrue();
        }
    }
}
