using System;
using System.Collections.Generic;
using System.Linq;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;

namespace PersonApi.V1.Boundary.Request
{
    public class CreatePersonRequestObject
    {
        public Guid Id { get; set; }

        public Title? Title { get; set; }

        public Title? PreferredTitle { get; set; }

        public string PreferredFirstName { get; set; }

        public string PreferredMiddleName { get; set; }

        public string PreferredSurname { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string PlaceOfBirth { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Reason { get; set; }
        public IEnumerable<PersonType> PersonTypes { get; set; }
        public IEnumerable<Tenure> Tenures { get; set; }
    }
}
