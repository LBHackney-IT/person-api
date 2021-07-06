using PersonApi.V1.Domain;

namespace PersonApi.V1.Gateways
{
    public class UpdatePersonGatewayResult
    {
        public UpdatePersonGatewayResult(Person old, Person updated)
        {
            OldPerson = old;
            UpdatedPerson = updated;
        }

        public Person OldPerson { get; private set; }
        public Person UpdatedPerson { get; private set; }
    }
}
