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
                "eyJhbGciOiJIUzM4NCIsInR5cCI6IkpXVCJ9.eyJncm91cHMiOiJlMmUtdGVzdGluZy1kZXZlbG9wbWVudCIsImVtYWlsIjoiZTJlLXRlc3R" +
                "pbmctZGV2ZWxvcG1lbnRAaGFja25leS5nb3YudWsiLCJuYW1lIjoiZTJlLXRlc3RpbmctZGV2ZWxvcG1lbnQiLCJuYmYiOjE2MjIwMTk4NTgsImV4cCI6MT" +
                "kzNzU1MjY1OCwiaWF0IjoxNjIyMDE5ODU4fQ.SoUUGRHkHxSqEfS0gXu2CT_lZtK2IwKLEJc2QfKWA4qGq9LmjnGbanM-5H-J9Xz-";
            var headerDictionary = new HeaderDictionary();
            headerDictionary.Add("Authorization", token);

            // Act
            var result = _sut.Create(headerDictionary);

            // Assert
            result.Email.Should().Be(expectedEmail);
        }
    }
}
