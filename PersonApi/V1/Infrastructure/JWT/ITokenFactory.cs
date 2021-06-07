using Microsoft.AspNetCore.Http;

namespace PersonApi.V1.Infrastructure.JWT
{
    public interface ITokenFactory
    {
        Token Create(IHeaderDictionary headerDictionary);
    }
}
