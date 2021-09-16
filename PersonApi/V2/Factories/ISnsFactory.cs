using Hackney.Core.JWT;
using Hackney.Shared.Person;

namespace PersonApi.V2.Factories
{
    public interface ISnsFactory
    {
        PersonSns Create(Person person, Token token);
    }
}
