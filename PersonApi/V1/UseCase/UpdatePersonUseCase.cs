using Hackney.Core.Logging;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.Infrastructure.JWT;
using PersonApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase
{
    public class UpdatePersonUseCase : IUpdatePersonUseCase
    {
        private readonly IPersonApiGateway _gateway;
        private readonly IResponseFactory _responseFactory;
        private readonly ISnsGateway _snsGateway;
        private readonly ISnsFactory _snsFactory;

        public UpdatePersonUseCase(IPersonApiGateway gateway, IResponseFactory responseFactory,
            ISnsGateway snsGateway, ISnsFactory snsFactory)
        {
            _gateway = gateway;
            _responseFactory = responseFactory;
            _snsFactory = snsFactory;
            _snsGateway = snsGateway;
        }

        [LogCall]
        public async Task<PersonResponseObject> ExecuteAsync(PersonRequestObject personRequestObject, Token token)
        {
            var person = await _gateway.UpdatePersonByIdAsync(personRequestObject).ConfigureAwait(false);

            var personSnsMessage = _snsFactory.Update(person, token);
            await _snsGateway.UpdatePersonPublish(personSnsMessage).ConfigureAwait(false);

            return _responseFactory.ToResponse(person);
        }
    }
}
