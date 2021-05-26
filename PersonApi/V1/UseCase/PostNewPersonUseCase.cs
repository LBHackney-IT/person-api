using System.Threading.Tasks;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase.Interfaces;

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
}
