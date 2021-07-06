using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure.JWT;

namespace PersonApi.V1.Factories
{
    public interface ISnsFactory
    {
        PersonSns Create(Person person, Token token);

        PersonSns Update(Person old, Person updated, Token token);
    }
}
