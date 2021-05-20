using Microsoft.Extensions.Logging;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.Logging;
using PersonApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
using PersonApi.V1.Boundary.Request;

namespace PersonApi.V1.UseCase
{
    public class PostNewPersonUseCase : IPostNewPersonUseCase
    {
        private readonly IPersonApiGateway _gateway;
        private readonly IResponseFactory _responseFactory;

        public PostNewPersonUseCase(IPersonApiGateway gateway, IResponseFactory responseFactory)
        {
            _gateway = gateway;
            _responseFactory = responseFactory;
        }

        public async Task<PersonResponseObject> ExecuteAsync(PersonRequestObject personRequestObject)
        {
            var person = await _gateway.PostNewPersonAsync(personRequestObject).
                ConfigureAwait(false);

            return _responseFactory.ToResponse(person);
        }
    }

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
        public async Task<PersonResponseObject> ExecuteAsync(Guid id)
        {
            var person = await _gateway.GetPersonByIdAsync(id).ConfigureAwait(false);
            return _responseFactory.ToResponse(person);
        }
    }
}
