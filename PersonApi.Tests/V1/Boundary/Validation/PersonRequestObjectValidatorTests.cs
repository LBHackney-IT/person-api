using AutoFixture;
using FluentValidation.TestHelper;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Request.Validation;
using PersonApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PersonApi.Tests.V1.Boundary.Request.Validation
{
    public class PersonRequestObjectValidatorTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly PersonRequestObjectValidator _sut;

        public PersonRequestObjectValidatorTests()
        {
            _sut = new PersonRequestObjectValidator();
        }

        private static IEnumerable<object[]> GetEnumValues<T>() where T : Enum
        {
            foreach (var val in Enum.GetValues(typeof(T)))
            {
                yield return new object[] { val };
            }
        }

        public static IEnumerable<object[]> Titles => GetEnumValues<Title>();
        public static IEnumerable<object[]> Genders => GetEnumValues<Gender>();

        //[Fact]
        //public void ShouldErrorWithEmptyId()
        //{
        //    var query = new PersonRequestObject() { Id = Guid.Empty };
        //    var result = _sut.TestValidate(query);
        //    result.ShouldHaveValidationErrorFor(x => x.Id);
        //}

        [Theory]
        [MemberData(nameof(Titles))]
        public void TitleShouldNotErrorWithValidValue(Title valid)
        {
            var model = new PersonRequestObject() { Title = valid };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [InlineData(100)]
        public void TitleShouldErrorWithInvalidValue(int? val)
        {
            var model = new PersonRequestObject() { Title = (Title) val };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [MemberData(nameof(Titles))]
        [InlineData(null)]
        public void PreferredTitleShouldNotErrorWithValidValue(Title? valid)
        {
            var model = new PersonRequestObject() { PreferredTitle = valid };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredTitle);
        }

        [Theory]
        [InlineData(100)]
        public void PreferredTitleShouldErrorWithInvalidValue(int? val)
        {
            var model = new PersonRequestObject() { PreferredTitle = (Title) val };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredTitle);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PreferredFirstnameShouldNotErrorWithNoValue(string value)
        {
            var model = new PersonRequestObject() { PreferredFirstname = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredFirstname);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PreferredFirstnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { PreferredFirstname = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredFirstname);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PreferredMiddleNameShouldNotErrorWithNoValue(string value)
        {
            var model = new PersonRequestObject() { PreferredMiddleName = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredMiddleName);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PreferredMiddleNameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { PreferredMiddleName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredMiddleName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PreferredSurnameShouldNotErrorWithNoValue(string value)
        {
            var model = new PersonRequestObject() { PreferredSurname = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredSurname);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PreferredSurnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { PreferredSurname = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredSurname);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Some string with <tag> in it.")]
        public void FirstnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { Firstname = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Firstname);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MiddleNameShouldNotErrorWithNoValue(string value)
        {
            var model = new PersonRequestObject() { MiddleName = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void MiddleNameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { MiddleName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MiddleName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Some string with <tag> in it.")]
        public void SurnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { Surname = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Surname);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void EthnicityShouldNotErrorWithNoValue(string value)
        {
            var model = new PersonRequestObject() { Ethnicity = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Ethnicity);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void EthnicityShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { Ethnicity = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Ethnicity);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NationalityShouldNotErrorWithNoValue(string value)
        {
            var model = new PersonRequestObject() { Nationality = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Nationality);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void NationalityShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { Nationality = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Nationality);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("NZ335598D")]
        [InlineData("nz335598d")]
        public void NationalInsuranceNoShouldNotErrorWithValidValue(string value)
        {
            var model = new PersonRequestObject() { NationalInsuranceNo = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.NationalInsuranceNo);
        }

        [Theory]
        [InlineData("BG335598D")]
        [InlineData("bg335598d")]
        [InlineData("fghdfhfgh")]
        public void NationalInsuranceNoShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { NationalInsuranceNo = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.NationalInsuranceNo);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PlaceOfBirthShouldNotErrorWithNoValue(string value)
        {
            var model = new PersonRequestObject() { PlaceOfBirth = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PlaceOfBirth);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PlaceOfBirthShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { PlaceOfBirth = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PlaceOfBirth);
        }

        [Fact]
        public void DateOfBirthShouldErrorWithEmptyValue()
        {
            var model = new PersonRequestObject() { DateOfBirth = default };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Fact]
        public void DateOfBirthShouldErrorWithFutureValue()
        {
            var model = new PersonRequestObject() { DateOfBirth = DateTime.UtcNow.AddDays(1) };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Theory]
        [MemberData(nameof(Genders))]
        public void GenderShouldNotErrorWithValidValue(Gender valid)
        {
            var model = new PersonRequestObject() { Gender = valid };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Gender);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Some string with <tag> in it.")]
        public void ReasonShouldErrorWithInvalidValue(string invalid)
        {
            var model = new PersonRequestObject() { Reason = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Reason);
        }

        [Fact]
        public void LanguagesShouldErrorWithTooMany()
        {
            var languages = _fixture.Build<Language>()
                                    .With(x => x.IsPrimary, false)
                                    .CreateMany(10).ToList();
            languages.Add(new Language() { Name = "Primary", IsPrimary = true });
            var model = new PersonRequestObject() { Languages = languages };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Languages);
        }

        [Fact]
        public void LanguagesShouldErrorWithNoPrimary()
        {
            var languages = _fixture.Build<Language>()
                                    .With(x => x.IsPrimary, false)
                                    .CreateMany(5).ToList();
            var model = new PersonRequestObject() { Languages = languages };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Languages);
        }

        [Fact]
        public void LanguagesShouldErrorWithTooManyPrimary()
        {
            var languages = _fixture.Build<Language>()
                                    .With(x => x.IsPrimary, true)
                                    .CreateMany(5).ToList();
            var model = new PersonRequestObject() { Languages = languages };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Languages);
        }

        [Fact]
        public void LanguagesShouldNotErrorWhenNull()
        {
            var model = new PersonRequestObject() { Languages = null };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Languages);
        }

        [Fact]
        public void LanguagesShouldNotErrorWhenEmpty()
        {
            var model = new PersonRequestObject() { Languages = Enumerable.Empty<Language>() };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Languages);
        }

        [Fact]
        public void PersonTypesShouldErrorWhenNull()
        {
            var model = new PersonRequestObject() { PersonTypes = null };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PersonTypes);
        }

        [Fact]
        public void PersonTypesShouldErrorWhenEmpty()
        {
            var model = new PersonRequestObject() { PersonTypes = Enumerable.Empty<PersonType>() };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PersonTypes);
        }

        [Fact]
        public void PersonTypesShouldErrorWithInvalidValue()
        {
            var model = new PersonRequestObject() { PersonTypes = new PersonType[] { (PersonType) 100 } };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PersonTypes);
        }

        [Theory]
        [InlineData(PersonType.HouseholdMember)]
        [InlineData(PersonType.Tenant)]
        [InlineData(PersonType.Tenant, PersonType.HouseholdMember)]
        public void PersonTypesShouldNotErrorWithValidValue(params PersonType[] types)
        {
            var model = new PersonRequestObject() { PersonTypes = types };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PersonTypes);
        }

        [Fact]
        public void CommunicationRequirementsShouldNotErrorWhenNull()
        {
            var model = new PersonRequestObject() { CommunicationRequirements = null };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.CommunicationRequirements);
        }

        [Fact]
        public void CommunicationRequirementsShouldNotErrorWhenEmpty()
        {
            var model = new PersonRequestObject() { CommunicationRequirements = Enumerable.Empty<CommunicationRequirement>() };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.CommunicationRequirements);
        }

        [Fact]
        public void CommunicationRequirementsShouldErrorWithInvalidValue()
        {
            var model = new PersonRequestObject() { CommunicationRequirements = new[] { (CommunicationRequirement) 100 } };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CommunicationRequirements);
        }

        [Theory]
        [InlineData(CommunicationRequirement.InterpreterRequired)]
        [InlineData(CommunicationRequirement.SignLanguage)]
        [InlineData(CommunicationRequirement.InterpreterRequired, CommunicationRequirement.SignLanguage)]
        public void CommunicationRequirementsShouldNotErrorWithValidValue(params CommunicationRequirement[] crs)
        {
            var model = new PersonRequestObject() { CommunicationRequirements = crs };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.CommunicationRequirements);
        }
    }
}
