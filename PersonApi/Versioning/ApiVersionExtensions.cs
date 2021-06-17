using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace PersonApi.Versioning
{
    [ExcludeFromCodeCoverage]
    public static class ApiVersionExtensions
    {
        public static string GetFormattedApiVersion(this ApiVersion apiVersion)
        {
            return $"v{apiVersion.ToString()}";
        }
    }
}
