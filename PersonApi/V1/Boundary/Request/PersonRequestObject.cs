using System;
using System.Collections.Generic;
using System.Linq;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;

namespace PersonApi.V1.Boundary.Request
{
    public class PersonRequestObject
    {
        public Guid Id { get; set; }

        public Title Title { get; set; }

        public Title PreferredTitle { get; set; }

        public string PreferredFirstname { get; set; }

        public string PreferredMiddleName { get; set; }

        public string PreferredSurname { get; set; }

        public string Firstname { get; set; }

        public string MiddleName { get; set; }

        public string Surname { get; set; }

        public string Ethnicity { get; set; }

        public string Nationality { get; set; }

        public string NationalInsuranceNo { get; set; }

        public string PlaceOfBirth { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string Reason { get; set; }

        public IEnumerable<Identification> Identifications { get; set; }

        public IEnumerable<Language> Languages { get; set; }

        public IEnumerable<CommunicationRequirement> CommunicationRequirements { get; set; }
        public IEnumerable<PersonType> PersonTypes { get; set; }
        public IEnumerable<Tenure> Tenures { get; set; }

        public PersonDbEntity ToDatabase()
        {
            return new PersonDbEntity()
            {
                Id = Id == Guid.Empty ? Guid.NewGuid() : Id,
                Title = Title,
                PreferredTitle = PreferredTitle,
                PreferredMiddleName = PreferredMiddleName,
                PreferredFirstname = PreferredFirstname,
                PreferredSurname = PreferredSurname,
                Firstname = Firstname,
                MiddleName = MiddleName,
                Surname = Surname,
                Ethnicity = Ethnicity,
                Nationality = Nationality,
                NationalInsuranceNo = NationalInsuranceNo,
                PlaceOfBirth = PlaceOfBirth,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                Reason = Reason,
                Identifications = Identifications == null ? new List<Identification>() : Identifications.Select(x => x).ToList(),
                Languages = Languages == null ? new List<Language>() : Languages.Select(x => x).ToList(),
                CommunicationRequirements = CommunicationRequirements == null ? new List<CommunicationRequirement>() : CommunicationRequirements.Select(x => x).ToList(),
                PersonTypes = PersonTypes == null ? new List<PersonType>() : PersonTypes.Select(x => x).ToList(),
                Tenures = Tenures == null ? new List<Tenure>() : Tenures.Select(x => x).ToList()
            };
        }
    }
}
