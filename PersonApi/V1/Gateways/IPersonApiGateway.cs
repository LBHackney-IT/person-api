using PersonApi.V1.Domain;

namespace PersonApi.V1.Gateways
{
    public interface IPersonApiGateway
    {
        Person GetEntityById(string id);
    }
}
