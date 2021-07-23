using Hackney.Core.JWT;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IUpdatePersonUseCase
    {
        Task<PersonResponseObject> ExecuteAsync(UpdatePersonRequestObject personRequestObject, string requestBody, Token token,
                                                PersonQueryObject query, int? ifMatch);
    }
}
