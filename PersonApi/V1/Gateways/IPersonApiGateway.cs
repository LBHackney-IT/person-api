using PersonApi.V1.Domain;
using System.Collections.Generic;

namespace PersonApi.V1.Gateways
{
    public interface IPersonApiGateway
    {
        Person GetEntityById(string id);

        List<Person> GetAll();
    }
}
