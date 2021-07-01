using System;
using System.Collections.Generic;
using System.Linq;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;

namespace PersonApi.V1.Boundary.Request
{
    public class UpdatePersonRequestObject
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

        public string Ethnicity { get; set; }

        public string Nationality { get; set; }

        public string NationalInsuranceNo { get; set; }

        public string PlaceOfBirth { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

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
                PreferredFirstName = PreferredFirstName,
                PreferredSurname = PreferredSurname,
                FirstName = FirstName,
                MiddleName = MiddleName,
                Surname = Surname,
                Ethnicity = Ethnicity,
                Nationality = Nationality,
                NationalInsuranceNo = NationalInsuranceNo?.ToUpper(),
                PlaceOfBirth = PlaceOfBirth,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                Identifications = Identifications == null ? null : Identifications.Select(x => x).ToList(),
                Languages = Languages == null ? null : Languages.Select(x => x).ToList(),
                CommunicationRequirements = CommunicationRequirements == null ? null : CommunicationRequirements.Select(x => x).ToList(),
                PersonTypes = PersonTypes == null ? null : PersonTypes.Select(x => x).ToList(),
                Tenures = Tenures == null ? null : Tenures.Select(x => x).ToList()
            };
        }
    }
}
