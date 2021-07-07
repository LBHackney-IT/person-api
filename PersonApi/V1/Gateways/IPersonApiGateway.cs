using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Domain;
using System.Threading.Tasks;

namespace PersonApi.V1.Gateways
{
    public interface IPersonApiGateway
    {
        Task<Person> GetPersonByIdAsync(PersonQueryObject query);

        Task<Person> PostNewPersonAsync(CreatePersonRequestObject requestObject);

        Task<UpdatePersonGatewayResult> UpdatePersonByIdAsync(UpdatePersonRequestObject requestObject, PersonQueryObject query);
    }
}
