using System.Threading.Tasks;
using Hackney.Shared.Person;

namespace PersonApi.V1.Gateways
{
    public interface ISnsGateway
    {
        Task Publish(PersonSns personSns);

    }
}
