using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi.V1.Infrastructure
{
    public static class UpdatePersonConstants
    {
        // JWT TOKEN
        public const string V1VERSION = "v1";
        public const string EVENTTYPE = "PersonUpdatedEvent";
        public const string SOURCEDOMAIN = "Person";
        public const string SOURCESYSTEM = "PersonAPI";
    }
}
