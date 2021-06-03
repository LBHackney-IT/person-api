using Hackney.Core.Logging;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase
{
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private readonly IPersonApiGateway _gateway;
        private readonly IResponseFactory _responseFactory;

        public GetByIdUseCase(IPersonApiGateway gateway, IResponseFactory responseFactory)
        {
            _gateway = gateway;
            _responseFactory = responseFactory;
        }

        [LogCall]
        public async Task<PersonResponseObject> ExecuteAsync(PersonQueryObject query)
        {
            var person = await _gateway.GetPersonByIdAsync(query).ConfigureAwait(false);
            return _responseFactory.ToResponse(person);
        }
    }
}
