using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        Task<Person> ExecuteAsync(PersonQueryObject query);
    }
}
