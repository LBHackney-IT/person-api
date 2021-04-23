using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PersonApi.V1
{
    public static class LoggingExtensions
    {
        public static IServiceCollection RegisterLogging(this IServiceCollection services)
        {
            services.AddScoped<IApiLogger, ApiLogger>();
            services.AddScoped<IMethodLogger, MethodLogger>();
            return services;
        }
    }

    public interface IApiLogger
    {
        void Log(LogLevel level, string message, Exception exception = null);
    }

    public class ApiLogger : IApiLogger
    {
        public void Log(LogLevel level, string message, Exception exception = null)
        {
            var logEntry = new LogEntry
            {
                Level = level,
                Message = message,
                ExceptionObject = exception,
                CorrelationId = CallContext.Current?.CorrelationId,
                UserId = CallContext.Current?.UserId,
                Timestamp = DateTime.UtcNow
            };

            LambdaLogger.Log(logEntry.ToString());
        }
    }


    public class LogEntry
    {
        private const string DATEFORMAT = "yyyy-MM-ddTHH\\:mm\\:ss.fffffffZ";

        public Guid? CorrelationId { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public Exception ExceptionObject { get; set; }
        public string Exception => ExceptionObject?.ToString();

        public override string ToString()
        {
            // TODO: Do we format this as a plain string or as JSON?

            string entry = $"{Timestamp.ToString(DATEFORMAT)} - {Level}: {CorrelationId} User: {UserId ?? "<unknown>"}. Message: {Message}";
            if (null != ExceptionObject)
                entry += $"{Environment.NewLine}Exception: {Exception}";
            return entry;

            //return this.ToJson();
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, GetJsonOptions());
        }

        private static JsonSerializerOptions _jsonOptions;
        private static JsonSerializerOptions GetJsonOptions()
        {
            if (null == _jsonOptions)
            {
                _jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                _jsonOptions.Converters.Add(new JsonStringEnumConverter());
            }
            return _jsonOptions;
        }
    }
}
