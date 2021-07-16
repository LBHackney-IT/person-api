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
        public const string PLACEOFBIRTH = "London";
        public static DateTime DATEOFBIRTH { get; } = DateTime.UtcNow.AddYears(-40);

        public const string SOMEUPRN = "SomeUprn";
        public const string SOMETYPE = "SomeType";
        public const string ASSETID = "SomeId";
        public const string ASSETFULLADDRESS = "SomeAddress";
        public const string STARTDATE = "2012-07-19";
        public const string ENDDATE = "2015-07-19";

        public static IEnumerable<PersonType> PERSONTYPES { get; }
            = new List<PersonType> { PersonType.HouseholdMember };

        public static Person ConstructPersonFromConstants()
        {
            var entity = new Person();
            entity.Id = Constants.ID;
            entity.Title = Constants.TITLE;
            entity.PreferredTitle = Constants.PREFTITLE;
            entity.PreferredFirstName = Constants.PREFFIRSTNAME;
            entity.PreferredMiddleName = Constants.PREFMIDDLENAME;
            entity.PreferredSurname = Constants.PREFSURNAME;
            entity.FirstName = Constants.FIRSTNAME;
            entity.MiddleName = Constants.MIDDLENAME;
            entity.Surname = Constants.SURNAME;
            entity.PlaceOfBirth = Constants.PLACEOFBIRTH;
            entity.DateOfBirth = Constants.DATEOFBIRTH;
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
            entity.PersonTypes = Constants.PERSONTYPES;
            return entity;
        }
    }
}
