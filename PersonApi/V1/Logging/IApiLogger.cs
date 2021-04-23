using Microsoft.Extensions.Logging;
using System;

namespace PersonApi.V1.Logging
{
    public interface IApiLogger
    {
        void Log(LogLevel level, string message, Exception exception = null);
    }
}
