using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PersonApi.V1.Logging
{
    public interface IMethodLogger
    {
        void Execute(string methodDesc, Action func);
        T Execute<T>(string methodDesc, Func<T> func);
        Task ExecuteAsync(string methodDesc, Func<Task> func);
        Task<T> ExecuteAsync<T>(string methodDesc, Func<Task<T>> func);
    }
    public class MethodLogger : IMethodLogger
    {
        private readonly IApiLogger _apiLogger;

        public MethodLogger(IApiLogger apiLogger)
        {
            _apiLogger = apiLogger;
        }

        private static void Validate(string methodDesc, object func)
        {
            if (string.IsNullOrEmpty(methodDesc)) throw new ArgumentNullException(nameof(func));
            if (null == func) throw new ArgumentNullException(nameof(func));
        }

        private void LogStart(string methodDesc)
        {
            _apiLogger.Log(LogLevel.Information, $"{methodDesc} STARTING");
        }
        private void LogEnd(string methodDesc)
        {
            _apiLogger.Log(LogLevel.Information, $"{methodDesc} ENDING");
        }

        public void Execute(string methodDesc, Action func)
        {
            Validate(methodDesc, func);

            LogStart(methodDesc);
            try
            {
                func();
            }
            finally
            {
                LogEnd(methodDesc);
            }
        }

        public T Execute<T>(string methodDesc, Func<T> func)
        {
            Validate(methodDesc, func);

            LogStart(methodDesc);
            try
            {
                return func();
            }
            finally
            {
                LogEnd(methodDesc);
            }
        }

        public async Task ExecuteAsync(string methodDesc, Func<Task> func)
        {
            Validate(methodDesc, func);

            LogStart(methodDesc);
            try
            {
                await func().ConfigureAwait(false);
            }
            finally
            {
                LogEnd(methodDesc);
            }
        }

        public async Task<T> ExecuteAsync<T>(string methodDesc, Func<Task<T>> func)
        {
            Validate(methodDesc, func);

            LogStart(methodDesc);
            try
            {
                return await func().ConfigureAwait(false);
            }
            finally
            {
                LogEnd(methodDesc);
            }
        }
    }
}
