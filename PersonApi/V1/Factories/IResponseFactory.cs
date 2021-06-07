using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;

namespace PersonApi.V1.Factories
{
    public interface IResponseFactory
    {
        PersonResponseObject ToResponse(Person domain);
    }
}
