using System.Threading.Tasks;
using PersonApi.V1.Domain;

namespace PersonApi.V1.Gateways
{
    public interface ISnsGateway
    {
        Task NewPersonPublish(PersonSns personSns);

        Task UpdatePersonPublish(PersonSns personSns);
    }
}
