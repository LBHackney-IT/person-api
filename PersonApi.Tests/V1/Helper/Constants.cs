using PersonApi.V1.Domain;
using System;
using System.Collections.Generic;

namespace PersonApi.Tests.V1.Helper
{
    public static class Constants
    {
        public static Guid ID { get; } = Guid.NewGuid();
        public const Title TITLE = Title.Mr;
        public const Title PREFTITLE = Title.Mr;
        public const string PREFFIRSTNAME = "Bob";
        public const string PREFMIDDLENAME = "Tim";
        public const string PREFSURNAME = "Roberts";
        public const string FIRSTNAME = "Robert";
        public const string MIDDLENAME = "Tim";
        public const string SURNAME = "Roberts";
        public const string ETHNICITY = "Caucasian";
        public const string NATIONALITY = "British";
        public const string NINO = "AA123456C";
        public const string PLACEOFBIRTH = "London";
        public static DateTime DATEOFBIRTH { get; } = DateTime.UtcNow.AddYears(-40);
        public const Gender GENDER = Gender.M;
        public const IdentificationType IDENTIFICATIONTYPE = IdentificationType.Passport;
        public const string IDENTIFICATIONVALUE = "Some id value";
        public const bool IDENTIFICATIONISSEEN = true;
        public const string IDENTIFICATIONLINKTODOC = "Some link";

        public const bool LANGUAGEISPRIMARY = true;
        public const string LANGUAGENAME = "English";

        public const string SOMEUPRN = "SomeUprn";
        public const string SOMETYPE = "SomeType";
        public const string ASSETID = "SomeId";
        public const string ASSETFULLADDRESS = "SomeAddress";
        public const string STARTDATE = "2012-07-19";
        public const string ENDDATE = "2015-07-19";

        public static IEnumerable<CommunicationRequirement> COMMSREQ { get; }
            = new List<CommunicationRequirement> { CommunicationRequirement.SignLanguage };
        public static IEnumerable<PersonType> PERSONTYPES { get; }
            = new List<PersonType> { PersonType.HouseholdMember };

        public static Person ConstructPersonFromConstants()
        {
            var entity = new Person();
            entity.Id = Constants.ID;
            entity.Title = Constants.TITLE;
            entity.PreferredTitle = Constants.PREFTITLE;
            entity.PreferredFirstname = Constants.PREFFIRSTNAME;
            entity.PreferredMiddleName = Constants.PREFMIDDLENAME;
            entity.PreferredSurname = Constants.PREFSURNAME;
            entity.Firstname = Constants.FIRSTNAME;
            entity.MiddleName = Constants.MIDDLENAME;
            entity.Surname = Constants.SURNAME;
            entity.Ethnicity = Constants.ETHNICITY;
            entity.Nationality = Constants.NATIONALITY;
            entity.NationalInsuranceNo = Constants.NINO;
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

            entity.Tenures = new[]
            {
                new Tenure
                {
                    AssetFullAddress = ASSETFULLADDRESS,
                    AssetId = ASSETID,
                    EndDate = ENDDATE,
                    StartDate = STARTDATE,
                    Id = Guid.NewGuid(),
                    Type = SOMETYPE,
                    Uprn = SOMEUPRN
                }
            };

            entity.CommunicationRequirements = Constants.COMMSREQ;
            entity.PersonTypes = Constants.PERSONTYPES;
            return entity;
        }
    }
}
