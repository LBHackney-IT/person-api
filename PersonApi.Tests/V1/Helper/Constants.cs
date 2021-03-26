using PersonApi.V1.Domain;
using System;
using System.Collections.Generic;

namespace PersonApi.Tests.V1.Helper
{
    public static class Constants
    {
        public static Guid ID { get; } = Guid.NewGuid();
        public const Title TITLE = Title.Mr;
        public const string PREFIRSTNAME = "Bob";
        public const string PREFSURNAME = "Roberts";
        public const string FIRSTNAME = "Robert";
        public const string MIDDLENAME = "Tim";
        public const string SURNAME = "Roberts";
        public const string ETHNICITY = "Caucasian";
        public const string NATIONALITY = "British";
        public const string PLACEOFBIRTH = "London";
        public static DateTime DATEOFBIRTH { get; } = DateTime.UtcNow.AddYears(-40);
        public const Gender GENDER = Gender.M;

        public const IdentificationType IDENTIFICATIONTYPE = IdentificationType.NI;
        public const string IDENTIFICATIONVALUE = "Some id value";
        public const bool IDENTIFICATIONISSEEN = true;
        public const string IDENTIFICATIONLINKTODOC = "Some link";

        public const bool LANGUAGEISPRIMARY = true;
        public const string LANGUAGENAME = "English";

        public static IEnumerable<CommunicationRequirement> COMMSREQ { get; }
            = new List<CommunicationRequirement> { CommunicationRequirement.SignLanguage };
        public static IEnumerable<PersonType> PERSONTYPES { get; }
            = new List<PersonType> { PersonType.HousingOfficer };

        public static Person ConstructPersonFromConstants()
        {
            var entity = new Person();
            entity.Id = Constants.ID;
            entity.Title = Constants.TITLE;
            entity.PreferredFirstname = Constants.PREFIRSTNAME;
            entity.PreferredSurname = Constants.PREFSURNAME;
            entity.Firstname = Constants.FIRSTNAME;
            entity.MiddleName = Constants.MIDDLENAME;
            entity.Surname = Constants.SURNAME;
            entity.Ethinicity = Constants.ETHNICITY;
            entity.Nationality = Constants.NATIONALITY;
            entity.PlaceOfBirth = Constants.PLACEOFBIRTH;
            entity.DateOfBirth = Constants.DATEOFBIRTH;
            entity.Gender = Constants.GENDER;
            entity.Identifications = new[]
            {
                new Identification
                {
                    IdentificationType = Constants.IDENTIFICATIONTYPE,
                    IsOriginalDocumentSeen = Constants.IDENTIFICATIONISSEEN,
                    LinkToDocument = Constants.IDENTIFICATIONLINKTODOC,
                    Value = Constants.IDENTIFICATIONVALUE
                }
            };
            entity.Languages = new[]
            {
                new Language
                {
                    IsPrimary = Constants.LANGUAGEISPRIMARY,
                    Name = Constants.LANGUAGENAME
                }
            };
            entity.CommunicationRequirements = Constants.COMMSREQ;
            entity.PersonTypes = Constants.PERSONTYPES;
            return entity;
        }
    }
}
