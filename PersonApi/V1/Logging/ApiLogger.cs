using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Context;
using System;

namespace PersonApi.V1.Logging
{
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
}
