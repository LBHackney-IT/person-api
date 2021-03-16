using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi.V1.Domain
{
    public class CorrelationNotFoundException : Exception
    {
        public CorrelationNotFoundException(string message) : base(message)
        {

        }

    }
}
