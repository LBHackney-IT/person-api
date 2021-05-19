using PersonApi.V1.Boundary.Response;
using System;
using System.Threading.Tasks;
using PersonApi.V1.Boundary.Request;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        Task<PersonResponseObject> ExecuteAsync(Guid id);
    }

    public interface IPostNewPersonUseCase
    {
        Task<PersonResponseObject> ExecuteAsync(PersonRequestObject personRequestObject);
    }
}
