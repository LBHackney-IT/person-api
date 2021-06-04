using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        Task<PersonResponseObject> ExecuteAsync(PersonQueryObject query);
    }
}
