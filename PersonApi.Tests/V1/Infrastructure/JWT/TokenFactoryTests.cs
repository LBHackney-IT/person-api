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
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMTUwMTgxMTYwOTIwOTg2NzYxMTMiLCJlbWFpbCI6ImUyZS10ZXN0aW5nLWR" +
                "ldmVsb3BtZW50QGhhY2tuZXkuZ292LnVrIiwiaXNzIjoiSGFja25leSIsIm5hbWUiOiJUZXN0ZXIiLCJncm91cHMiOlsic2FtbC1hd3MtY29u" +
                "c29sZS1tdGZoLWRldmVsb3BlciJdLCJpYXQiOjE2MjMwNTgyMzJ9.WffAEwWJlQorHGf-rIwxET8cJFK2yZg-kxNbtFctav4";

            var headerDictionary = new HeaderDictionary();
            headerDictionary.Add("Authorization", token);

            // Act
            var result = _sut.Create(headerDictionary);

            // Assert
            result.Email.Should().Be(expectedEmail);
        }
    }
}
