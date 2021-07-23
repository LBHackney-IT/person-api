using FluentValidation;
using Hackney.Core.Validation;
using System;

namespace PersonApi.V1.Boundary.Request.Validation
{
    public class UpdatePersonRequestObjectValidator : AbstractValidator<UpdatePersonRequestObject>
    {

        public UpdatePersonRequestObjectValidator()
        {
            // Mandatory fields
            RuleFor(x => x.Title).IsInEnum();
            RuleFor(x => x.DateOfBirth).NotEqual(default(DateTime))
                                       .LessThan(DateTime.UtcNow);
            RuleFor(x => x.FirstName).NotXssString();
            RuleFor(x => x.Surname).NotXssString();

            // Others
            RuleFor(x => x.MiddleName).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.MiddleName));
            RuleFor(x => x.PreferredTitle).IsInEnum()
                .When(y => y.PreferredTitle != null);
            RuleFor(x => x.PreferredFirstName).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.PreferredFirstName));
            RuleFor(x => x.PreferredMiddleName).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.PreferredMiddleName));
            RuleFor(x => x.PreferredSurname).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.PreferredSurname));
            RuleFor(x => x.PlaceOfBirth).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.PlaceOfBirth));
            RuleForEach(x => x.Tenures).SetValidator(new TenureValidator());
        }
    }
}
