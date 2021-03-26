using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace PersonApi.V1.Boundary.Response
{
    public class ApiLink
    {
        public Uri Href { get; set; }
        public string Rel { get; set; }
        public HttpVerb EndpointType { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
