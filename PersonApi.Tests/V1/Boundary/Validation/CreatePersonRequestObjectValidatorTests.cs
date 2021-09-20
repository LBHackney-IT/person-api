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

        private const string StringWithTags = "Some string with <tag> in it.";

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

        [Fact]
        public void PreferredFirstnameShouldErrorWithTagsInValue()
        {
            var model = new CreatePersonRequestObject() { PreferredFirstName = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredFirstName)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
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

        [Fact]
        public void PreferredMiddleNameShouldErrorWithTagsInValue()
        {
            var model = new CreatePersonRequestObject() { PreferredMiddleName = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredMiddleName)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
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

        [Fact]
        public void PreferredSurnameShouldErrorWithTagsInValue()
        {
            var model = new CreatePersonRequestObject() { PreferredSurname = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredSurname)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FirstnameShouldErrorWithNoValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { FirstName = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorCode(ErrorCodes.FirstNameMandatory);
        }

        [Fact]
        public void FirstnameShouldErrorWithWithTagsInValue()
        {
            var model = new CreatePersonRequestObject() { FirstName = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
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

        [Fact]
        public void MiddleNameShouldErrorWithTagsInValue()
        {
            var model = new CreatePersonRequestObject() { MiddleName = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MiddleName)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SurnameShouldErrorWithNoValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { Surname = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Surname)
                  .WithErrorCode(ErrorCodes.SurnameMandatory);
        }

        [Fact]
        public void SurnameShouldErrorWithTagsInValue()
        {
            var model = new CreatePersonRequestObject() { Surname = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Surname)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
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

        [Fact]
        public void PlaceOfBirthShouldErrorWithTagsInValue()
        {
            var model = new CreatePersonRequestObject() { PlaceOfBirth = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PlaceOfBirth)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Fact]
        public void DateOfBirthShouldErrorWithEmptyValue()
        {
            var model = new CreatePersonRequestObject() { DateOfBirth = default(DateTime) };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                  .WithErrorCode(ErrorCodes.DoBInvalid);
        }

        [Fact]
        public void DateOfBirthShouldErrorWithFutureValue()
        {
            var model = new CreatePersonRequestObject() { DateOfBirth = DateTime.UtcNow.AddDays(1) };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                  .WithErrorCode(ErrorCodes.DoBInFuture);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReasonShouldErrorWithNoValue(string invalid)
        {
            var model = new CreatePersonRequestObject() { Reason = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Reason)
                  .WithErrorCode(ErrorCodes.ReasonMandatory);
        }

        [Fact]
        public void ReasonShouldErrorWithTagsInValue()
        {
            var model = new CreatePersonRequestObject() { Reason = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Reason)
                  .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Fact]
        public void PersonTypesShouldErrorWhenNull()
        {
            var model = new CreatePersonRequestObject() { PersonTypes = null };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PersonTypes)
                  .WithErrorCode(ErrorCodes.PersonTypeMandatory);
        }

        [Fact]
        public void PersonTypesShouldErrorWhenEmpty()
        {
            var model = new CreatePersonRequestObject() { PersonTypes = Enumerable.Empty<PersonType>() };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PersonTypes)
                  .WithErrorCode(ErrorCodes.PersonTypeMandatory);
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
        [InlineData(PersonType.Tenant, PersonType.HouseholdMember, PersonType.Freeholder, PersonType.Leaseholder, PersonType.Occupant)]
        public void PersonTypesShouldNotErrorWithValidValue(params PersonType[] types)
        {
            var model = new CreatePersonRequestObject() { PersonTypes = types };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PersonTypes);
        }
    }
}
