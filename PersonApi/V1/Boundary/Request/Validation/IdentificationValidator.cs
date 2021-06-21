using FluentValidation;
using Hackney.Core.Validation;
using PersonApi.V1.Domain;

namespace PersonApi.V1.Boundary.Request.Validation
{
    public class IdentificationValidator : AbstractValidator<Identification>
    {
        public IdentificationValidator()
        {
            RuleFor(x => x.IdentificationType).NotNull()
                                              .IsInEnum();
            RuleFor(y => y.Value).NotNull()
                                 .NotEmpty()
                                 .NotXssString();
        }
    }
}
