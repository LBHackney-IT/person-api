using FluentValidation.TestHelper;
using PersonApi.V1.Boundary.Request.Validation;
using PersonApi.V1.Domain;
using Xunit;

namespace PersonApi.Tests.V1.Boundary.Request.Validation
{
    public class IdentificationValidatorTests
    {
        private readonly IdentificationValidator _sut;

        public IdentificationValidatorTests()
        {
            _sut = new IdentificationValidator();
        }

        [Theory]
        [InlineData(IdentificationType.DrivingLicence)]
        [InlineData(IdentificationType.Passport)]
        public void IdentificationTypeShouldNotErrorWithValidValue(IdentificationType invalid)
        {
            var model = new Identification() { IdentificationType = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.IdentificationType);
        }

        [Theory]
        [InlineData(100)]
        public void IdentificationTypeShouldErrorWithInvalidValue(int? val)
        {
            var model = new Identification() { IdentificationType = (IdentificationType) val };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.IdentificationType);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Some string with <tag> in it.")]
        public void ValueShouldErrorWithInvalidValue(string invalid)
        {
            var model = new Identification() { Value = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Value);
        }
    }
}
