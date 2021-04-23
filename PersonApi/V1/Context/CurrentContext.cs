using PersonApi.V1.Controllers;
using System;
using System.Collections.Generic;

namespace PersonApi.V1.Context
{
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
