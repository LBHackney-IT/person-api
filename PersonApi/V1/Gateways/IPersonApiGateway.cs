using Hackney.Shared.Person;
using Hackney.Shared.Person.Boundary.Request;
using Hackney.Shared.Person.Infrastructure;
using PersonApi.V1.Infrastructure;
using System.Threading.Tasks;

namespace PersonApi.V1.Gateways
{
    public interface IPersonApiGateway
    {
        Task<Person> GetPersonByIdAsync(PersonQueryObject query);

        Task<Person> PostNewPersonAsync(CreatePersonRequestObject requestObject);

        Task<UpdateEntityResult<PersonDbEntity>> UpdatePersonByIdAsync(UpdatePersonRequestObject requestObject, string requestBody,
                                                                       PersonQueryObject query, int? ifMatch);
    }
}
