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
    public class UpdatePersonRequestObjectValidatorTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly UpdatePersonRequestObjectValidator _sut;

        public UpdatePersonRequestObjectValidatorTests()
        {
            _sut = new UpdatePersonRequestObjectValidator();
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
            var model = new UpdatePersonRequestObject() { Title = valid };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [InlineData(100)]
        public void TitleShouldErrorWithInvalidValue(int? val)
        {
            var model = new UpdatePersonRequestObject() { Title = (Title) val };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [MemberData(nameof(Titles))]
        [InlineData(null)]
        public void PreferredTitleShouldNotErrorWithValidValue(Title? valid)
        {
            var model = new UpdatePersonRequestObject() { PreferredTitle = valid };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredTitle);
        }

        [Theory]
        [InlineData(100)]
        public void PreferredTitleShouldErrorWithInvalidValue(int? val)
        {
            var model = new UpdatePersonRequestObject() { PreferredTitle = (Title) val };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredTitle);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PreferredFirstnameShouldNotErrorWithNoValue(string value)
        {
            var model = new UpdatePersonRequestObject() { PreferredFirstName = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredFirstName);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PreferredFirstnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new UpdatePersonRequestObject() { PreferredFirstName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredFirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PreferredMiddleNameShouldNotErrorWithNoValue(string value)
        {
            var model = new UpdatePersonRequestObject() { PreferredMiddleName = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredMiddleName);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PreferredMiddleNameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new UpdatePersonRequestObject() { PreferredMiddleName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredMiddleName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PreferredSurnameShouldNotErrorWithNoValue(string value)
        {
            var model = new UpdatePersonRequestObject() { PreferredSurname = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredSurname);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PreferredSurnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new UpdatePersonRequestObject() { PreferredSurname = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredSurname);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void FirstnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new UpdatePersonRequestObject() { FirstName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MiddleNameShouldNotErrorWithNoValue(string value)
        {
            var model = new UpdatePersonRequestObject() { MiddleName = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void MiddleNameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new UpdatePersonRequestObject() { MiddleName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MiddleName);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void SurnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new UpdatePersonRequestObject() { Surname = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Surname);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void EthnicityShouldNotErrorWithNoValue(string value)
        {
            var model = new UpdatePersonRequestObject() { Ethnicity = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Ethnicity);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void EthnicityShouldErrorWithInvalidValue(string invalid)
        {
            var model = new UpdatePersonRequestObject() { Ethnicity = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Ethnicity);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NationalityShouldNotErrorWithNoValue(string value)
        {
            var model = new UpdatePersonRequestObject() { Nationality = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Nationality);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void NationalityShouldErrorWithInvalidValue(string invalid)
        {
            var model = new UpdatePersonRequestObject() { Nationality = invalid };
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
            var model = new UpdatePersonRequestObject() { NationalInsuranceNo = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.NationalInsuranceNo);
        }

        [Theory]
        [InlineData("BG335598D")]
        [InlineData("bg335598d")]
        [InlineData("fghdfhfgh")]
        public void NationalInsuranceNoShouldErrorWithInvalidValue(string invalid)
        {
            var model = new UpdatePersonRequestObject() { NationalInsuranceNo = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.NationalInsuranceNo);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PlaceOfBirthShouldNotErrorWithNoValue(string value)
        {
            var model = new UpdatePersonRequestObject() { PlaceOfBirth = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PlaceOfBirth);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PlaceOfBirthShouldErrorWithInvalidValue(string invalid)
        {
            var model = new UpdatePersonRequestObject() { PlaceOfBirth = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PlaceOfBirth);
        }


        [Fact]
        public void DateOfBirthShouldErrorWithFutureValue()
        {
            var model = new UpdatePersonRequestObject() { DateOfBirth = DateTime.UtcNow.AddDays(1) };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Theory]
        [MemberData(nameof(Genders))]
        public void GenderShouldNotErrorWithValidValue(Gender valid)
        {
            var model = new UpdatePersonRequestObject() { Gender = valid };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Gender);
        }

        [Theory]
        [InlineData(100)]
        public void GenderShouldErrorWithInvalidValue(int? val)
        {
            var model = new UpdatePersonRequestObject() { Gender = (Gender) val };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Gender);
        }

        [Fact]
        public void LanguagesShouldErrorWithTooMany()
        {
            var languages = _fixture.Build<Language>()
                                    .With(x => x.IsPrimary, false)
                                    .CreateMany(10).ToList();
            languages.Add(new Language() { Name = "Primary", IsPrimary = true });
            var model = new UpdatePersonRequestObject() { Languages = languages };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Languages);
        }

        [Fact]
        public void LanguagesShouldErrorWithNoPrimary()
        {
            var languages = _fixture.Build<Language>()
                                    .With(x => x.IsPrimary, false)
                                    .CreateMany(5).ToList();
            var model = new UpdatePersonRequestObject() { Languages = languages };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Languages);
        }

        [Fact]
        public void LanguagesShouldErrorWithTooManyPrimary()
        {
            var languages = _fixture.Build<Language>()
                                    .With(x => x.IsPrimary, true)
                                    .CreateMany(5).ToList();
            var model = new UpdatePersonRequestObject() { Languages = languages };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Languages);
        }

        [Fact]
        public void LanguagesShouldNotErrorWhenNull()
        {
            var model = new UpdatePersonRequestObject() { Languages = null };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Languages);
        }

        [Fact]
        public void LanguagesShouldNotErrorWhenEmpty()
        {
            var model = new UpdatePersonRequestObject() { Languages = Enumerable.Empty<Language>() };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Languages);
        }

        [Fact]
        public void PersonTypesShouldErrorWithInvalidValue()
        {
            var model = new UpdatePersonRequestObject() { PersonTypes = new PersonType[] { (PersonType) 100 } };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PersonTypes);
        }

        [Theory]
        [InlineData(PersonType.HouseholdMember)]
        [InlineData(PersonType.Tenant)]
        [InlineData(PersonType.Tenant, PersonType.HouseholdMember)]
        public void PersonTypesShouldNotErrorWithValidValue(params PersonType[] types)
        {
            var model = new UpdatePersonRequestObject() { PersonTypes = types };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PersonTypes);
        }

        [Fact]
        public void CommunicationRequirementsShouldNotErrorWhenNull()
        {
            var model = new UpdatePersonRequestObject() { CommunicationRequirements = null };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.CommunicationRequirements);
        }

        [Fact]
        public void CommunicationRequirementsShouldNotErrorWhenEmpty()
        {
            var model = new UpdatePersonRequestObject() { CommunicationRequirements = Enumerable.Empty<CommunicationRequirement>() };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.CommunicationRequirements);
        }

        [Fact]
        public void CommunicationRequirementsShouldErrorWithInvalidValue()
        {
            var model = new UpdatePersonRequestObject() { CommunicationRequirements = new[] { (CommunicationRequirement) 100 } };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CommunicationRequirements);
        }

        [Theory]
        [InlineData(CommunicationRequirement.InterpreterRequired)]
        [InlineData(CommunicationRequirement.SignLanguage)]
        [InlineData(CommunicationRequirement.InterpreterRequired, CommunicationRequirement.SignLanguage)]
        public void CommunicationRequirementsShouldNotErrorWithValidValue(params CommunicationRequirement[] crs)
        {
            var model = new UpdatePersonRequestObject() { CommunicationRequirements = crs };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.CommunicationRequirements);
        }
    }
}
