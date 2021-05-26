using PersonApi.V1.Domain;
using System;
using System.Collections.Generic;

namespace PersonApi.V1.Boundary.Response
{
    public class PersonResponseObject
    {
        public Guid Id { get; set; }

        /// <example>Mr, Mrs, Miss</example>
        public Title Title { get; set; }
        /// <example>Julie</example>
        public string PreferredFirstName { get; set; }
        /// <example>Evans</example>
        public string PreferredSurname { get; set; }
        /// <example>Julie</example>
        public string FirstName { get; set; }
        /// <example></example>
        public string MiddleName { get; set; }
        /// <example>Evans</example>
        public string Surname { get; set; }
        /// <example>Caucasian</example>
        public string Ethnicity { get; set; }
        /// <example>British</example>
        public string Nationality { get; set; }
        /// <example>London</example>
        public string PlaceOfBirth { get; set; }
        /// <example>1990-02-19</example>
        public string DateOfBirth { get; set; }
        /// <example>M, F</example>
        public string Reason { get; set; }
        public Gender Gender { get; set; }
        public IEnumerable<Identification> Identifications { get; set; }
        public IEnumerable<Language> Languages { get; set; }
        public IEnumerable<CommunicationRequirement> CommunicationRequirements { get; set; }
        public IEnumerable<PersonType> PersonTypes { get; set; }
        public IEnumerable<ApiLink> Links { get; set; }
        public IEnumerable<Tenure> Tenures { get; set; }
    }
}
