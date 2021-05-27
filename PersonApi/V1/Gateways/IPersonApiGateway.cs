using PersonApi.V1.Domain;
using System;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Auth;
using PersonApi.V1.Boundary.Request;

namespace PersonApi.V1.Gateways
{
    public interface IPersonApiGateway
    {
        Task<Person> GetPersonByIdAsync(Guid id);

        Task<Person> PostNewPersonAsync(PersonRequestObject requestObject);
    }
}
