using System.Collections.Generic;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;

namespace PersonApi.V1.Gateways
{
    //TODO: Rename to match the data source that is being accessed in the gateway eg. MosaicGateway
    public class PersonApiGateway : IPersonApiGateway
    {
        private readonly DatabaseContext _databaseContext;

        public PersonApiGateway(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Entity GetEntityById(int id)
        {
            var result = _databaseContext.DatabaseEntities.Find(id);

            return result?.ToDomain();
        }

        public List<Entity> GetAll()
        {
            return new List<Entity>();
        }
    }
}
