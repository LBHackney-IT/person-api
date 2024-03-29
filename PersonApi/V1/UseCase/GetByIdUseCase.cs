using Hackney.Core.Logging;
using Hackney.Shared.Person;
using Hackney.Shared.Person.Boundary.Request;
using PersonApi.V1.Gateways;
using PersonApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase
{
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private readonly IPersonApiGateway _gateway;

        public GetByIdUseCase(IPersonApiGateway gateway)
        {
            _gateway = gateway;
        }

        [LogCall]
        public async Task<Person> ExecuteAsync(PersonQueryObject query)
        {
            var person = await _gateway.GetPersonByIdAsync(query).ConfigureAwait(false);
            return person;
        }
    }
}
