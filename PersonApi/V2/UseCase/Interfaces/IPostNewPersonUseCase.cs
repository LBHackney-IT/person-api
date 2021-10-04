using Hackney.Core.JWT;
using Hackney.Shared.Person.Boundary.Request;
using Hackney.Shared.Person.Boundary.Response;
using System.Threading.Tasks;

namespace PersonApi.V2.UseCase.Interfaces
{
    public interface IPostNewPersonUseCase
    {
        Task<PersonResponseObject> ExecuteAsync(CreatePersonRequestObject personRequestObject, Token token);
    }
}
