using PersonApi.V1.Controllers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleToAttribute("PersonApi.Tests")]

namespace PersonApi.V1
{
    public static class CallContext
    {
        private static AsyncLocal<CurrentContext> _currentContext = new AsyncLocal<CurrentContext>();

        public static ICurrentContext Current => _currentContext.Value;
        public static EventHandler<ICurrentContext> OnCurrentChanged { get; set; }
        public static void ClearCurrentContext()
        {
            _currentContext.Value = null;
            if (null != OnCurrentChanged) OnCurrentChanged(null, _currentContext.Value);
        }

        public static void NewContext()
        {
            _currentContext.Value = new CurrentContext();
            if (null != OnCurrentChanged) OnCurrentChanged(null, _currentContext.Value);
        }
    }

    public interface ICurrentContext
    {
        Guid? CorrelationId { get; }
        string UserId { get; }

        object GetValue(string key);
        void SetValue(string key, object value);
    }

    internal class CurrentContext : ICurrentContext
    {
        private Dictionary<string, object> _contextValues { get; set; } = new Dictionary<string, object>();

        public Guid? CorrelationId
        {
            get
            {
                var val = GetValue(Constants.CorrelationId);

                if (null == val) return null;
                return (Guid) val;
            }
            set { SetValue(Constants.CorrelationId, value); }
        }

        public string UserId
        {
            get { return GetValue(Constants.UserId)?.ToString() ?? default; }
            set { SetValue(Constants.UserId, value); }
        }

        public object GetValue(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            return _contextValues.ContainsKey(key) ? _contextValues[key] : null;
        }

        public void SetValue(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            _contextValues[key] = value;
        }
    }

}
