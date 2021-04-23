using Microsoft.Extensions.DependencyInjection;
using System;

namespace PersonApi.V1.Logging
{
    public static class LoggingExtensions
    {
        public static IServiceCollection RegisterLogging(this IServiceCollection services)
        {
            if (null == services) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IApiLogger, ApiLogger>();
            services.AddScoped<IMethodLogger, MethodLogger>();
            return services;
        }
    }
}
