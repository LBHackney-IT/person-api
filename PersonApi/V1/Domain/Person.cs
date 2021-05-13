using System;
using System.Collections.Generic;

namespace PersonApi.V1.Domain
{
    public class Person
    {
        public Guid Id { get; set; }
        public Title Title { get; set; }
        public string PreferredFirstname { get; set; }
        public string PreferredSurname { get; set; }
        public string Firstname { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string Ethnicity { get; set; }
        public string Nationality { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public IEnumerable<Identification> Identifications { get; set; }
        public IEnumerable<Language> Languages { get; set; }
        public IEnumerable<CommunicationRequirement> CommunicationRequirements { get; set; }
        public IEnumerable<PersonType> PersonTypes { get; set; }
        public IEnumerable<Tenure> Tenures { get; set; }
    }
}
