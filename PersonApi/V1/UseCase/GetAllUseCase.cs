using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase.Interfaces;

namespace PersonApi.V1.UseCase
{
    //TODO: Rename class name and interface name to reflect the entity they are representing eg. GetAllClaimantsUseCase
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly IPersonApiGateway _gateway;
        public GetAllUseCase(IPersonApiGateway gateway)
        {
            _gateway = gateway;
        }

        public ResponseObjectList Execute()
        {
            return new ResponseObjectList { ResponseObjects = _gateway.GetAll().ToResponse() };
        }
    }
}
