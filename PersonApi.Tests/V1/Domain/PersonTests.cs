using FluentAssertions;
using NUnit.Framework;
using PersonApi.Tests.V1.Helper;
using PersonApi.V1.Domain;
using System.Linq;

namespace PersonApi.Tests.V1.Domain
{
    [TestFixture]
    public class PersonTests
    {
        [Test]
        public void PersonHasPropertiesSet()
        {
            Person person = Constants.ConstructPersonFromConstants();

            person.Id.Should().Be(Constants.ID);
            person.Title.Should().Be(Constants.TITLE);
            person.PreferredFirstname.Should().Be(Constants.PREFIRSTNAME);
            person.PreferredSurname.Should().Be(Constants.PREFSURNAME);
            person.Firstname.Should().Be(Constants.FIRSTNAME);
            person.MiddleName.Should().Be(Constants.MIDDLENAME);
            person.Surname.Should().Be(Constants.SURNAME);
            person.Ethnicity.Should().Be(Constants.ETHNICITY);
            person.Nationality.Should().Be(Constants.NATIONALITY);
            person.PlaceOfBirth.Should().Be(Constants.PLACEOFBIRTH);
            person.DateOfBirth.Should().Be(Constants.DATEOFBIRTH);
            person.Gender.Should().Be(Constants.GENDER);
            person.Identifications.Should().ContainSingle();
            person.Identifications.First().IdentificationType.Should().Be(Constants.IDENTIFICATIONTYPE);
            person.Identifications.First().IsOriginalDocumentSeen.Should().Be(Constants.IDENTIFICATIONISSEEN);
            person.Identifications.First().LinkToDocument.Should().Be(Constants.IDENTIFICATIONLINKTODOC);
            person.Identifications.First().Value.Should().Be(Constants.IDENTIFICATIONVALUE);
            person.Languages.Should().ContainSingle();
            person.Languages.First().IsPrimary.Should().Be(Constants.LANGUAGEISPRIMARY);
            person.Languages.First().Name.Should().Be(Constants.LANGUAGENAME);
            person.CommunicationRequirements.Should().BeEquivalentTo(Constants.COMMSREQ);
            person.PersonTypes.Should().BeEquivalentTo(Constants.PERSONTYPES);
        }
    }
}
