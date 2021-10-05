using Hackney.Core.JWT;
using Hackney.Shared.Person.Boundary.Request;
using Hackney.Shared.Person.Boundary.Response;
using PersonApi.V2.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V2.UseCase.Interfaces;
using System.Threading.Tasks;
using Hackney.Shared.Person.Factories;

namespace PersonApi.V2.UseCase
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

        public async Task<PersonResponseObject> ExecuteAsync(CreatePersonRequestObject personRequestObject, Token token)
        {
            var person = await _gateway.PostNewPersonAsync(personRequestObject).ConfigureAwait(false);

            var personSnsMessage = _snsFactory.Create(person, token);
            await _snsGateway.Publish(personSnsMessage).ConfigureAwait(false);

            return _responseFactory.ToResponse(person);
        }
    }
}
