using FluentValidation;
using Hackney.Core.Validation;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PersonApi.V1.Boundary.Request.Validation
{
    public class UpdatePersonRequestObjectValidator : AbstractValidator<UpdatePersonRequestObject>
    {
        private const string NiRegEx
            = @"^(?!BG)(?!GB)(?!NK)(?!KN)(?!TN)(?!NT)(?!ZZ)(?:[A-CEGHJ-PR-TW-Z][A-CEGHJ-NPR-TW-Z])(?:\s*\d\s*){6}([A-D]|\s)$";

        public UpdatePersonRequestObjectValidator()
        {
            // Mandatory fields
            RuleFor(x => x.Title).IsInEnum();
            RuleFor(x => x.DateOfBirth).NotEqual(default(DateTime))
                                       .LessThan(DateTime.UtcNow);
            RuleFor(x => x.FirstName).NotXssString();
            RuleFor(x => x.Surname).NotXssString();
            RuleFor(x => x.Gender).IsInEnum();
            RuleFor(x => x.NationalInsuranceNo)
                                 .Matches(NiRegEx, RegexOptions.IgnoreCase)
                                 .When(x => !string.IsNullOrEmpty(x.NationalInsuranceNo));
            RuleFor(x => x.Languages).Must(x => x.Count() < 10)
                                     .WithMessage("Please only enter up to 10 languages")
                                     .When(x => x.Languages != null);
            RuleForEach(x => x.Languages).SetValidator(new LanguageValidator());
            RuleFor(x => x.Languages).Must(x => x.Count(y => y.IsPrimary) == 1)
                                     .WithMessage("You must choose one language as the primary")
                                     .When(x => (x.Languages != null) && x.Languages.Any());

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
            RuleFor(x => x.Ethnicity).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.Ethnicity));
            RuleFor(x => x.Nationality).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.Nationality));
            RuleFor(x => x.PlaceOfBirth).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.PlaceOfBirth));
            RuleForEach(x => x.Identifications).SetValidator(new IdentificationValidator());
            RuleForEach(x => x.CommunicationRequirements)
                .ChildRules(x => x.RuleFor(y => y).IsInEnum());
            RuleForEach(x => x.Tenures).SetValidator(new TenureValidator());
        }
    }
}
