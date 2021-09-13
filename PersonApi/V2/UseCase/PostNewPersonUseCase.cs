using Hackney.Core.JWT;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V2.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V2.UseCase.Interfaces;
using System.Threading.Tasks;
using V1Factories = PersonApi.V1.Factories;

namespace PersonApi.V2.UseCase
{
    public class PostNewPersonUseCase : IPostNewPersonUseCase
    {
        private readonly IPersonApiGateway _gateway;
        private readonly V1Factories.IResponseFactory _responseFactory;
        private readonly ISnsGateway _snsGateway;
        private readonly ISnsFactory _snsFactory;

        public PostNewPersonUseCase(IPersonApiGateway gateway, V1Factories.IResponseFactory responseFactory,
            ISnsGateway snsGateway, ISnsFactory snsFactory)
        {
            _gateway = gateway;
            _responseFactory = responseFactory;
            _snsGateway = snsGateway;
            _snsFactory = snsFactory;
        }

        public async Task<PersonResponseObject> ExecuteAsync(CreatePersonRequestObject personRequestObject, Token token)
        {
            var person = await _gateway.PostNewPersonAsync(personRequestObject).ConfigureAwait(false);

            var personSnsMessage = _snsFactory.Create(person, token);
            await _snsGateway.Publish(personSnsMessage).ConfigureAwait(false);

            return _responseFactory.ToResponse(person);
        }
    }
}
