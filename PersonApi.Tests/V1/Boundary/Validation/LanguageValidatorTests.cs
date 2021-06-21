using FluentValidation.TestHelper;
using PersonApi.V1.Boundary.Request.Validation;
using PersonApi.V1.Domain;
using Xunit;

namespace PersonApi.Tests.V1.Boundary.Request.Validation
{
    public class LanguageValidatorTests
    {
        private readonly LanguageValidator _sut;

        public LanguageValidatorTests()
        {
            _sut = new LanguageValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Some string with <tag> in it.")]
        public void LanguageShouldErrorWithInvalidName(string invalid)
        {
            var model = new Language() { Name = invalid };
            var result = _sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}
