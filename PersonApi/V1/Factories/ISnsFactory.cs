using Hackney.Core.JWT;
using Hackney.Shared.Person;
using Hackney.Shared.Person.Infrastructure;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;

namespace PersonApi.V1.Factories
{
    public interface ISnsFactory
    {
        PersonSns Create(Person person, Token token);

        PersonSns Update(UpdateEntityResult<PersonDbEntity> updateResult, Token token);
    }
}
