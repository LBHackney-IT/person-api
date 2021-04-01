using PersonApi.V1.Domain;
using System;
using System.Threading.Tasks;

namespace PersonApi.V1.Gateways
{
    public interface IPersonApiGateway
    {
        Task<Person> GetPersonByIdAsync(Guid id);
    }
}
