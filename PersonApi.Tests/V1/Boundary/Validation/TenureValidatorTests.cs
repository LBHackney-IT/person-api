using FluentValidation.TestHelper;
using PersonApi.V1.Boundary.Request.Validation;
using PersonApi.V1.Domain;
using System;
using Xunit;

namespace PersonApi.Tests.V1.Boundary.Request.Validation
{
    public class TenureValidatorTests
    {
        private readonly TenureValidator _sut;

        public TenureValidatorTests()
        {
            _sut = new TenureValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddressShouldNotErrorWithNoValue(string value)
        {
            var model = new Tenure() { AssetFullAddress = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.AssetFullAddress);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void AssetIdShouldErrorWithInvalidValue(string invalid)
        {
            var model = new Tenure() { AssetId = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AssetId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AssetIdShouldNotErrorWithNoValue(string value)
        {
            var model = new Tenure() { AssetId = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.AssetId);
        }

        [Theory]
        [InlineData("10 Some month 2001")]
        [InlineData("Some string with <tag> in it.")]
        public void StartDateShouldErrorWithInvalidValue(string invalid)
        {
            var model = new Tenure() { StartDate = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.StartDate);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("2000-10-10")]
        public void StartDateShouldNotErrorWithNoValue(string value)
        {
            var model = new Tenure() { StartDate = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.StartDate);
        }

        [Theory]
        [InlineData("10 Some month 2001")]
        [InlineData("Some string with <tag> in it.")]
        public void EndDateShouldErrorWithInvalidValue(string invalid)
        {
            var model = new Tenure() { EndDate = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.EndDate);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("2000-10-10")]
        public void EndDateShouldNotErrorWithNoValue(string value)
        {
            var model = new Tenure() { EndDate = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.EndDate);
        }

        [Fact]
        public void ShouldErrorWithEmptyId()
        {
            var query = new Tenure() { Id = Guid.Empty };
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void AddressShouldErrorWithInvalidValue(string invalid)
        {
            var model = new Tenure() { AssetFullAddress = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AssetFullAddress);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void TypeShouldErrorWithInvalidValue(string invalid)
        {
            var model = new Tenure() { Type = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Type);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TypeShouldNotErrorWithNoValue(string value)
        {
            var model = new Tenure() { Type = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Type);
        }

        [Theory]
        [InlineData("Some string with <tag> in it.")]
        public void UprnShouldErrorWithInvalidValue(string invalid)
        {
            var model = new Tenure() { Uprn = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Uprn);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void UprnShouldNotErrorWithNoValue(string value)
        {
            var model = new Tenure() { Uprn = value };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Uprn);
        }
    }
}
