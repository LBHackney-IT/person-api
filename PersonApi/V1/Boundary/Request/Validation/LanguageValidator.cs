using FluentValidation;
using Hackney.Core.Validation;
using PersonApi.V1.Domain;

namespace PersonApi.V1.Boundary.Request.Validation
{
    public class LanguageValidator : AbstractValidator<Language>
    {
        public LanguageValidator()
        {
            RuleFor(y => y.Name).NotNull()
                                .NotEmpty()
                                .NotXssString();
        }
    }
}
