using FluentValidation;
using Hackney.Core.Validation;
using System;

namespace PersonApi.V1.Boundary.Request.Validation
{
    public class CreatePersonRequestObjectValidator : AbstractValidator<CreatePersonRequestObject>
    {

        public CreatePersonRequestObjectValidator()
        {
            // Not needed for create person, but will be needed for edit person
            //RuleFor(x => x.Id).NotNull()
            //                  .NotEqual(Guid.Empty);

            // Mandatory fields
            RuleFor(x => x.Title).NotNull()
                                 .IsInEnum();
            RuleFor(x => x.DateOfBirth).NotNull()
                                       .NotEqual(default(DateTime))
                                       .LessThan(DateTime.UtcNow);
            RuleFor(x => x.FirstName).NotNull()
                                     .NotEmpty()
                                     .NotXssString();
            RuleFor(x => x.Surname).NotNull()
                                   .NotEmpty()
                                   .NotXssString();

            RuleFor(x => x.PersonTypes).NotNull()
                                       .NotEmpty();
            RuleForEach(x => x.PersonTypes)
                .ChildRules(x => x.RuleFor(y => y).IsInEnum());

            RuleFor(x => x.Reason).NotNull()
                                  .NotEmpty()
                                  .NotXssString();

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
            RuleFor(x => x.Reason).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.Reason));
            RuleForEach(x => x.Tenures).SetValidator(new TenureValidator());
        }
    }
}
