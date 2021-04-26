using Microsoft.Extensions.Logging;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase
{
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private readonly IPersonApiGateway _gateway;
        private readonly IResponseFactory _responseFactory;
        private readonly ILogger<GetByIdUseCase> _logger;

        public GetByIdUseCase(IPersonApiGateway gateway, IResponseFactory responseFactory, ILogger<GetByIdUseCase> logger)
        {
            _gateway = gateway;
            _responseFactory = responseFactory;
            _logger = logger;
        }

        public async Task<PersonResponseObject> ExecuteAsync(Guid id)
        {
            _logger.LogTrace("Calling GetByIdUseCase.ExecuteAsync");

            var person = await _gateway.GetPersonByIdAsync(id).ConfigureAwait(false);
            return _responseFactory.ToResponse(person);
        }
    }
}
