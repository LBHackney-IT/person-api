using FluentValidation;
using Hackney.Core.Validation;
using Hackney.Shared.Person;
using System;

namespace PersonApi.V1.Boundary.Request.Validation
{
    public class TenureValidator : AbstractValidator<Tenure>
    {
        public TenureValidator()
        {
            RuleFor(x => x.AssetFullAddress)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.AssetFullAddress));
            RuleFor(x => x.AssetId)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.AssetId));
            RuleFor(x => x.StartDate).Must(x => DateTime.TryParse(x, out DateTime dt))
                .When(y => !string.IsNullOrEmpty(y.StartDate));
            RuleFor(x => x.EndDate).Must(x => DateTime.TryParse(x, out DateTime dt))
                .When(y => !string.IsNullOrEmpty(y.EndDate));
            RuleFor(x => x.Id).NotNull()
                              .NotEqual(Guid.Empty);
            RuleFor(x => x.Type)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.Type));
            RuleFor(x => x.Uprn)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.Uprn));
            RuleFor(x => x.PropertyReference)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.Uprn));
            RuleFor(x => x.PaymentReference)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(y => !string.IsNullOrEmpty(y.Uprn));
        }
    }
}
