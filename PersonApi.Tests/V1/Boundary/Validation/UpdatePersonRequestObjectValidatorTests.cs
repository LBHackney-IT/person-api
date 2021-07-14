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
    }
}
