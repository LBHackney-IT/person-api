using FluentAssertions;
using Microsoft.AspNetCore.Http;
using PersonApi.V1.Infrastructure.JWT;
using Xunit;

namespace PersonApi.Tests.V1.Infrastructure.JWT
{
    public class TokenFactoryTests
    {
        private TokenFactory _sut;

        public TokenFactoryTests()
        {
            _sut = new TokenFactory();
        }

        /// <summary>
        /// Use jwt.io to decode the token used for the test.
        /// You can then see that the expected email is indeed : e2e-testing-development@hackney.gov.uk
        /// </summary>
        [Fact]
        public void GivenAnAuthorizationTokenWhenProcessingShouldReturnCorrectEmailInformation()
        {
            // Arrange
            var expectedEmail = "e2e-testing-development@hackney.gov.uk";
            var token =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMTUwMTgxMTYwOTIwOTg2NzYxMTMiLCJlbWFpbCI6ImV2YW5nZWxvcy5ha3RvdWRpYW5ha" +
                "2lzQGhhY2tuZXkuZ292LnVrIiwiaXNzIjoiSGFja25leSIsIm5hbWUiOiJFdmFuZ2Vsb3MgQWt0b3VkaWFuYWtpcyIsImdyb3VwcyI6WyJzYW1sLWF3cy1jb25zb" +
                "2xlLW10ZmgtZGV2ZWxvcGVyIl0sImlhdCI6MTYyMzA1ODIzMn0.Jnd2kQTMiAUeKMJCYQVEVXbFc9BbIH90OociR15gfpw";
            var headerDictionary = new HeaderDictionary();
            headerDictionary.Add("Authorization", token);

            // Act
            var result = _sut.Create(headerDictionary);

            // Assert
            result.Email.Should().Be(expectedEmail);
        }
    }
}
