using Hackney.Core.JWT;
using PersonApi.V1.Domain;

namespace PersonApi.V2.Factories
{
    public interface ISnsFactory
    {
        PersonSns Create(Person person, Token token);
    }
}
