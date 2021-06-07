using Microsoft.AspNetCore.Http;

namespace PersonApi.V1.Infrastructure
{
    public class HttpContextWrapper : IHttpContextWrapper
    {
        public IHeaderDictionary GetContextRequestHeaders(HttpContext context)
        {
            return context.Request.Headers;
        }
    }
}
