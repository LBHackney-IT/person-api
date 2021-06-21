using System.Diagnostics.CodeAnalysis;

namespace PersonApi.V1.Domain.Configuration
{
    [ExcludeFromCodeCoverage]
    public class AwsConfiguration
    {
        public string Region { get; set; }
    }
}
