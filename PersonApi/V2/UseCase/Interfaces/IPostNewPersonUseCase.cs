using Hackney.Core.JWT;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace PersonApi.V2.UseCase.Interfaces
{
    public interface IPostNewPersonUseCase
    {
        Task<PersonResponseObject> ExecuteAsync(CreatePersonRequestObject personRequestObject, Token token);
    }
}
