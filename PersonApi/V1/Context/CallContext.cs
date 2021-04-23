using System;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleToAttribute("PersonApi.Tests")]

namespace PersonApi.V1.Context
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
}
