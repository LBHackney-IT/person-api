using System;

namespace PersonApi.V1.Domain
{
    public class PersonSns
    {
        public string Id { get; set; }

        public string EventType { get; set; }

        public string SourceDomain { get; set; }

        public string SourceSystem { get; set; }

        public string Version { get; set; }

        public string CorrelationId { get; set; }

        public DateTime DateTime { get; set; }

        public User User { get; set; }

        public string EntityId { get; set; }
    }
}
