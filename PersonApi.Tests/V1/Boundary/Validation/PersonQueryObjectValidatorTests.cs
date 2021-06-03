using FluentValidation.TestHelper;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Request.Validation;
using System;
using Xunit;

namespace PersonApi.Tests.V1.Boundary.Request.Validation
{
    public class PersonQueryObjectValidatorTests
    {
        private readonly PersonQueryObjectValidator _sut;

        public PersonQueryObjectValidatorTests()
        {
            _sut = new PersonQueryObjectValidator();
        }

        [Fact]
        public void QueryShouldErrorWithNullTargetId()
        {
            var query = new PersonQueryObject();
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void QueryShouldErrorWithEmptyTargetId()
        {
            var query = new PersonQueryObject() { Id = Guid.Empty };
            var result = _sut.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}
