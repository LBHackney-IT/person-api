using FluentValidation;
using System;

namespace PersonApi.V1.Boundary.Request.Validation
{
    public class PersonQueryObjectValidator : AbstractValidator<PersonQueryObject>
    {
        public PersonQueryObjectValidator()
        {
            RuleFor(x => x.Id).NotNull()
                              .NotEqual(Guid.Empty);
        }
    }
}
