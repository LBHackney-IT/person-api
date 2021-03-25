using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase.Interfaces;
using System;

namespace PersonApi.V1.UseCase
{
    //TODO: Rename class name and interface name to reflect the entity they are representing eg. GetClaimantByIdUseCase
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private readonly IPersonApiGateway _gateway;
        public GetByIdUseCase(IPersonApiGateway gateway)
        {
            _gateway = gateway;
        }

        public PersonResponseObject Execute(Guid id)
        {
            return _gateway.GetEntityById(id.ToString()).ToResponse();
        }
    }
}
