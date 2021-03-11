using System.Collections.Generic;
using PersonApi.V1.Domain;

namespace PersonApi.V1.Gateways
{
    public interface IPersonApiGateway
    {
        Entity GetEntityById(int id);

        List<Entity> GetAll();
    }
}
