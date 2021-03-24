using PersonApi.V1.Boundary.Response;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        ResponseObject Execute(string id);
    }
}
