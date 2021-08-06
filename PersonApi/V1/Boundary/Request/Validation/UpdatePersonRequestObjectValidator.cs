using FluentValidation;
using Hackney.Core.Validation;
using System;

namespace PersonApi.V1.Boundary.Request.Validation
{
    public class UpdatePersonRequestObjectValidator : AbstractValidator<UpdatePersonRequestObject>
    {
        public UpdatePersonRequestObjectValidator()
        {
            RuleFor(x => x.Title)
                .IsInEnum()
                .When(y => y.Title.HasValue);
            RuleFor(x => x.DateOfBirth)
                .NotEqual(default(DateTime))
                .WithErrorCode(ErrorCodes.DoBInvalid)
                .When(y => y.DateOfBirth.HasValue);
            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.UtcNow)
                .WithErrorCode(ErrorCodes.DoBInFuture)
                .When(y => y.DateOfBirth.HasValue);
            RuleFor(x => x.FirstName)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.FirstName));
            RuleFor(x => x.Surname)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.Surname));
            RuleFor(x => x.MiddleName)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.MiddleName));
            RuleFor(x => x.PreferredTitle).IsInEnum()
                .When(y => y.PreferredTitle != null);
            RuleFor(x => x.PreferredFirstName)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.PreferredFirstName));
            RuleFor(x => x.PreferredMiddleName)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.PreferredMiddleName));
            RuleFor(x => x.PreferredSurname)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.PreferredSurname));
            RuleFor(x => x.PlaceOfBirth)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.PlaceOfBirth));

            RuleForEach(x => x.Tenures).SetValidator(new TenureValidator());
        }
    }
}
