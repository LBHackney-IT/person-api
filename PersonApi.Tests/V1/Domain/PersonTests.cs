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
            Person entity = Constants.ConstructPersonFromConstants();

            entity.Id.Should().Be(Constants.ID);
            entity.Title.Should().Be(Constants.TITLE);
            entity.PreferredFirstname.Should().Be(Constants.PREFIRSTNAME);
            entity.PreferredSurname.Should().Be(Constants.PREFSURNAME);
            entity.Firstname.Should().Be(Constants.FIRSTNAME);
            entity.MiddleName.Should().Be(Constants.MIDDLENAME);
            entity.Surname.Should().Be(Constants.SURNAME);
            entity.Ethinicity.Should().Be(Constants.ETHNICITY);
            entity.Nationality.Should().Be(Constants.NATIONALITY);
            entity.PlaceOfBirth.Should().Be(Constants.PLACEOFBIRTH);
            entity.DateOfBirth.Should().Be(Constants.DATEOFBIRTH);
            entity.Gender.Should().Be(Constants.GENDER);
            entity.Identifications.Should().ContainSingle();
            entity.Identifications.First().IdentificationType.Should().Be(Constants.IDENTIFICATIONTYPE);
            entity.Identifications.First().IsOriginalDocumentSeen.Should().Be(Constants.IDENTIFICATIONISSEEN);
            entity.Identifications.First().LinkToDocument.Should().Be(Constants.IDENTIFICATIONLINKTODOC);
            entity.Identifications.First().Value.Should().Be(Constants.IDENTIFICATIONVALUE);
            entity.Languages.Should().ContainSingle();
            entity.Languages.First().IsPrimary.Should().Be(Constants.LANGUAGEISPRIMARY);
            entity.Languages.First().Name.Should().Be(Constants.LANGUAGENAME);
            entity.CommunicationRequirements.Should().BeEquivalentTo(Constants.COMMSREQ);
            entity.PersonTypes.Should().BeEquivalentTo(Constants.PERSONTYPES);
            entity.Links.Should().ContainSingle();
            entity.Links.First().EndpointType.Should().Be(Constants.LINKENDPOINTTYPE);
            entity.Links.First().Href.Should().Be(Constants.LINKHREF);
            entity.Links.First().Rel.Should().Be(Constants.LINKREL);
        }
    }
}
