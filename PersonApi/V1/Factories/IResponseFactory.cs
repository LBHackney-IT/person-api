using PersonApi.V1.Boundary.Response;
using Hackney.Shared.Person;

namespace PersonApi.V1.Factories
{
    public interface IResponseFactory
    {
        PersonResponseObject ToResponse(Person domain);

    }
}
