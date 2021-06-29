using System.Threading.Tasks;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Infrastructure.JWT;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IPostNewPersonUseCase
    {
        Task<PersonResponseObject> ExecuteAsync(CreatePersonRequestObject personRequestObject, Token token);
    }
}
