using System.Threading.Tasks;
using PersonApi.V1.Domain;

namespace PersonApi.V1.Gateways
{
    public interface ISnsGateway
    {
        Task Publish(PersonSns personSns);

    }
}
