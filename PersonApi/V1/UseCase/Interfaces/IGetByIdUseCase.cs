using PersonApi.V1.Boundary.Response;
using System;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        PersonResponseObject Execute(Guid id);
    }
}
