using Microsoft.AspNetCore.Http;

namespace PersonApi.V1.Infrastructure
{
    public interface IHttpContextWrapper
    {
        IHeaderDictionary GetContextRequestHeaders(HttpContext context);
    }
}
