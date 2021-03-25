using System.Collections.Generic;
using System.Linq;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;

namespace PersonApi.V1.Factories
{
    public static class ResponseFactory
    {
        //TODO: Map the fields in the domain object(s) to fields in the response object(s).
        // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings
        public static PersonResponseObject ToResponse(this Person domain)
        {
            return new PersonResponseObject();
        }

        public static List<PersonResponseObject> ToResponse(this IEnumerable<Person> domainList)
        {
            return domainList.Select(domain => domain.ToResponse()).ToList();
        }
    }
}
