using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.Infrastructure.JWT;
using PersonApi.V1.Logging;
using PersonApi.V1.UseCase.Interfaces;

namespace PersonApi.V1.UseCase
{
    public class PostNewPersonUseCase : IPostNewPersonUseCase
    {
        private readonly IPersonApiGateway _gateway;
        private readonly IResponseFactory _responseFactory;
        private readonly ISnsGateway _snsGateway;
        private readonly ISnsFactory _snsFactory;

        public PostNewPersonUseCase(IPersonApiGateway gateway, IResponseFactory responseFactory,
            ISnsGateway snsGateway, ISnsFactory snsFactory)
        {
            _gateway = gateway;
            _responseFactory = responseFactory;
            _snsGateway = snsGateway;
            _snsFactory = snsFactory;
        }

        [LogCall]
        public async Task<PersonResponseObject> ExecuteAsync(PersonRequestObject personRequestObject, Token token)
        {
            var person = await _gateway.PostNewPersonAsync(personRequestObject).ConfigureAwait(false);

            var personSns = _snsFactory.Create(person, Guid.NewGuid().ToString());
            await _snsGateway.Publish(personSns).ConfigureAwait(false);

            return _responseFactory.ToResponse(person);
        }
    }
}
