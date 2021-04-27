using Microsoft.AspNetCore.Builder;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PersonApi.V1.Logging
{
    [ExcludeFromCodeCoverage]
    public static class LogCallAspectServices
    {
        public static IApplicationBuilder UseLogCall(this IApplicationBuilder builder)
        {
            ServiceProvider = builder.ApplicationServices;
            return builder;
        }

        public static IServiceProvider ServiceProvider { get; private set; }

        static LogCallAspectServices() { }

        public static object GetInstance(Type type) => ServiceProvider.GetService(type);
    }
}
