using Hackney.Shared.Person;
using Hackney.Shared.Person.Boundary.Request;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        Task<Person> ExecuteAsync(PersonQueryObject query);
    }
}
