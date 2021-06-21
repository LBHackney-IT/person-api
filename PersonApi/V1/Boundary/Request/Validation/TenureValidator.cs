using FluentValidation;
using Hackney.Core.Validation;
using PersonApi.V1.Domain;
using System;

namespace PersonApi.V1.Boundary.Request.Validation
{
    public class TenureValidator : AbstractValidator<Tenure>
    {
        public TenureValidator()
        {
            RuleFor(x => x.AssetFullAddress).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.AssetFullAddress));
            RuleFor(x => x.AssetId).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.AssetId));
            RuleFor(x => x.StartDate).Must(x => DateTime.TryParse(x, out DateTime dt))
                .When(y => !string.IsNullOrEmpty(y.StartDate));
            RuleFor(x => x.EndDate).Must(x => DateTime.TryParse(x, out DateTime dt))
                .When(y => !string.IsNullOrEmpty(y.EndDate));
            RuleFor(x => x.Id).NotNull()
                              .NotEqual(Guid.Empty);
            RuleFor(x => x.Type).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.Type));
            RuleFor(x => x.Uprn).NotXssString()
                .When(y => !string.IsNullOrEmpty(y.Uprn));
        }
    }
}
