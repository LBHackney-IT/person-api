using FluentValidation.TestHelper;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Request.Validation;
using Hackney.Shared.Person;
using System;
using System.Collections.Generic;
using Xunit;

namespace PersonApi.Tests.V1.Boundary.Request.Validation
{
    public class UpdatePersonRequestObjectValidatorTests
    {
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

        private const string StringWithTags = "Some string with <tag> in it.";


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

        [Fact]
        public void PreferredFirstnameShouldErrorWithTagsInValue()
        {
            var model = new UpdatePersonRequestObject() { PreferredFirstName = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredFirstName)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
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

        [Fact]
        public void PreferredMiddleNameShouldErrorWithTagsInValue()
        {
            var model = new UpdatePersonRequestObject() { PreferredMiddleName = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredMiddleName)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
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

        [Fact]
        public void PreferredSurnameShouldErrorWithTagsInValue()
        {
            var model = new UpdatePersonRequestObject() { PreferredSurname = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PreferredSurname)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FirstNameShouldNotErrorWithNoValue(string value)
        {
            var model = new UpdatePersonRequestObject() { FirstName = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void FirstNameShouldErrorWithTagsInValue()
        {
            var model = new UpdatePersonRequestObject() { FirstName = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
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

        [Fact]
        public void MiddleNameShouldErrorWithTagsInValue()
        {
            var model = new UpdatePersonRequestObject() { MiddleName = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MiddleName)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SurnameShouldNotErrorWithNoValue(string value)
        {
            var model = new UpdatePersonRequestObject() { Surname = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Surname);
        }

        [Fact]
        public void SurnameShouldErrorWithTagsInValue()
        {
            var model = new UpdatePersonRequestObject() { Surname = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Surname)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
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

        [Fact]
        public void PlaceOfBirthShouldErrorWithTagsInValue()
        {
            var model = new UpdatePersonRequestObject() { PlaceOfBirth = StringWithTags };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PlaceOfBirth)
                .WithErrorCode(ErrorCodes.XssCheckFailure);
        }

        [Fact]
        public void DateOfBirthShouldNotErrorWithNoValue()
        {
            var model = new UpdatePersonRequestObject() { DateOfBirth = null };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Fact]
        public void DateOfBirthShouldErrorWithDefaultValue()
        {
            var model = new UpdatePersonRequestObject() { DateOfBirth = default(DateTime) };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                .WithErrorCode(ErrorCodes.DoBInvalid);
        }

        [Fact]
        public void DateOfBirthShouldErrorWithFutureValue()
        {
            var model = new UpdatePersonRequestObject() { DateOfBirth = DateTime.UtcNow.AddDays(1) };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                .WithErrorCode(ErrorCodes.DoBInFuture);
        }
    }
}
