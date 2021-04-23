using System;

namespace PersonApi.V1.Context
{
    public interface ICurrentContext
    {
        Guid? CorrelationId { get; }
        string UserId { get; }

        object GetValue(string key);
        void SetValue(string key, object value);
    }
}
