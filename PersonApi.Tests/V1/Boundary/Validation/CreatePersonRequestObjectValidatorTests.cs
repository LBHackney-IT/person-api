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
    public class CreatePersonRequestObjectValidatorTests
    {
        private readonly CreatePersonRequestObjectValidator _sut;

        public CreatePersonRequestObjectValidatorTests()
        {
            _sut = new CreatePersonRequestObjectValidator();
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

        [Theory]
        [MemberData(nameof(Titles))]
        public void TitleShouldNotErrorWithValidValue(Title valid)
        {
            var model = new CreatePersonRequestObject() { Title = valid };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [InlineData(100)]
        public void TitleShouldErrorWithInvalidValue(int? val)
        {
            var model = new CreatePersonRequestObject() { Title = (Title) val };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Theory]
        [MemberData(nameof(Titles))]
        [InlineData(null)]
        public void PreferredTitleShouldNotErrorWithValidValue(Title? valid)
        {
            var model = new CreatePersonRequestObject() { PreferredTitle = valid };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredTitle);
        }

        [Theory]
        [InlineData(100)]
        public void PreferredTitleShouldErrorWithInvalidValue(int? val)
        {
            var model = new CreatePersonRequestObject() { PreferredTitle = (Title) val };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredTitle);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PreferredFirstnameShouldNotErrorWithNoValue(string value)
        {
            var model = new CreatePersonRequestObject() { PreferredFirstName = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredFirstName);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PreferredFirstnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { PreferredFirstName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredFirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PreferredMiddleNameShouldNotErrorWithNoValue(string value)
        {
            var model = new CreatePersonRequestObject() { PreferredMiddleName = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredMiddleName);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PreferredMiddleNameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { PreferredMiddleName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredMiddleName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PreferredSurnameShouldNotErrorWithNoValue(string value)
        {
            var model = new CreatePersonRequestObject() { PreferredSurname = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PreferredSurname);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PreferredSurnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { PreferredSurname = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredSurname);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Some string with <tag> in it.")]
        public void FirstnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { FirstName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void MiddleNameShouldNotErrorWithNoValue(string value)
        {
            var model = new CreatePersonRequestObject() { MiddleName = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void MiddleNameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { MiddleName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MiddleName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Some string with <tag> in it.")]
        public void SurnameShouldErrorWithInvalidValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { Surname = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Surname);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PlaceOfBirthShouldNotErrorWithNoValue(string value)
        {
            var model = new CreatePersonRequestObject() { PlaceOfBirth = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PlaceOfBirth);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void PlaceOfBirthShouldErrorWithInvalidValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { PlaceOfBirth = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PlaceOfBirth);
        }

        [Fact]
        public void DateOfBirthShouldErrorWithEmptyValue()
        {
            var model = new CreatePersonRequestObject() { DateOfBirth = default };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Fact]
        public void DateOfBirthShouldErrorWithFutureValue()
        {
            var model = new CreatePersonRequestObject() { DateOfBirth = DateTime.UtcNow.AddDays(1) };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Some string with <tag> in it.")]
        public void ReasonShouldErrorWithInvalidValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { Reason = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Reason);
        }

        [Fact]
        public void PersonTypesShouldErrorWhenNull()
        {
            var model = new CreatePersonRequestObject() { PersonTypes = null };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PersonTypes);
        }

        [Fact]
        public void PersonTypesShouldErrorWhenEmpty()
        {
            var model = new CreatePersonRequestObject() { PersonTypes = Enumerable.Empty<PersonType>() };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PersonTypes);
        }

        [Fact]
        public void PersonTypesShouldErrorWithInvalidValue()
        {
            var model = new CreatePersonRequestObject() { PersonTypes = new PersonType[] { (PersonType) 100 } };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PersonTypes);
        }

        [Theory]
        [InlineData(PersonType.HouseholdMember)]
        [InlineData(PersonType.Tenant)]
        [InlineData(PersonType.Tenant, PersonType.HouseholdMember)]
        public void PersonTypesShouldNotErrorWithValidValue(params PersonType[] types)
        {
            var model = new CreatePersonRequestObject() { PersonTypes = types };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PersonTypes);
        }
    }
}
