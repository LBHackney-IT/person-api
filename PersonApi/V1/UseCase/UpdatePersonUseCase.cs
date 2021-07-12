using Hackney.Core.JWT;
using Hackney.Core.Logging;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase.Interfaces;
using System.Linq;
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
        public async Task<PersonResponseObject> ExecuteAsync(UpdatePersonRequestObject personRequestObject, string requestBody,
            Token token, PersonQueryObject query)
        {
            var result = await _gateway.UpdatePersonByIdAsync(personRequestObject, requestBody, query).ConfigureAwait(false);
            if (result is null) return null;

            // Only raise the event if something actually changed.
            if (result.NewValues.Any())
            {
                var personSnsMessage = _snsFactory.Update(result, token);
                await _snsGateway.Publish(personSnsMessage).ConfigureAwait(false);
            }

            return _responseFactory.ToResponse(result.UpdatedEntity.ToDomain());
        }
    }
}
