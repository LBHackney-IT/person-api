using System;

namespace PersonApi.V1.Domain
{
    public class ApiLink
    {
        public Uri Href { get; set; }
        public string Rel { get; set; }
        public HttpVerb EndpointType { get; set; }
    }
}
