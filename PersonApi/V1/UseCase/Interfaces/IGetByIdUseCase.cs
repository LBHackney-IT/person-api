using PersonApi.V1.Boundary.Response;
using System;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        Task<PersonResponseObject> ExecuteAsync(Guid id);
    }
}
