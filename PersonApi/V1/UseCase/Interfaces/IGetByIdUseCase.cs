using PersonApi.V1.Boundary.Request;
using Hackney.Shared.Person;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        Task<Person> ExecuteAsync(PersonQueryObject query);
    }
}
