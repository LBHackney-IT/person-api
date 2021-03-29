using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;
using System.Collections.Generic;
using System.Linq;

namespace PersonApi.V1.Boundary
{
    public interface IApiLinkGenerator
    {
        IEnumerable<ApiLink> GenerateLinksForPerson(Person person);
    }

    public class ApiLinkGenerator : IApiLinkGenerator
    {
        public IEnumerable<ApiLink> GenerateLinksForPerson(Person person)
        {
            // TODO - Implement link generation
            return Enumerable.Empty<ApiLink>();
        }
    }
}
