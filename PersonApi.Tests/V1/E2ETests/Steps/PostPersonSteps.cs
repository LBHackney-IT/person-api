using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PersonApi.V1.Domain;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PersonApi.Tests.V1.E2ETests.Steps
{
    public class PostPersonSteps : BaseSteps
    {
        public PostPersonSteps(HttpClient httpClient) : base(httpClient)
        { }

        /// <summary>
        /// You can use jwt.io to decode the token - it is the same one we'd use on dev, etc. 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task WhenTheCreatePersonApiIsCalled(CreatePersonRequestObject requestObject)
        {
            var token =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMTUwMTgxMTYwOTIwOTg2NzYxMTMiLCJlbWFpbCI6ImUyZS10ZXN0aW5nLWRldmVsb3BtZW50QGhhY2tuZXkuZ292LnVrIiwiaXNzIjoiSGFja25leSIsIm5hbWUiOiJUZXN0ZXIiLCJncm91cHMiOlsic2FtbC1hd3MtY29uc29sZS1tdGZoLWRldmVsb3BlciJdLCJpYXQiOjE2MjMwNTgyMzJ9.WffAEwWJlQorHGf-rIwxET8cJFK2yZg-kxNbtFctav4";
            var uri = new Uri($"api/v1/persons", UriKind.Relative);

            var message = new HttpRequestMessage(HttpMethod.Post, uri);

            message.Content = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Post;
            message.Headers.Add("Authorization", token);

            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _lastResponse = await _httpClient.SendAsync(message).ConfigureAwait(false);
        }

        public async Task ThenThePersonDetailsAreReturnedAndIdIsNotEmpty()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiPerson = JsonSerializer.Deserialize<PersonResponseObject>(responseContent, CreateJsonOptions());

            apiPerson.Id.Should().NotBeEmpty();
        }

        public async Task ThenTheValidationErrorsAreReturned()
        {
            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            JObject jo = JObject.Parse(responseContent);
            var errorProperties = jo["errors"].Children().Select(x => x.Path.Split('.').Last().Trim('\'', ']')).ToList();

            errorProperties.Should().Contain("Firstname");
            errorProperties.Should().Contain("Surname");
            errorProperties.Should().Contain("PersonTypes");
            errorProperties.Should().Contain("DateOfBirth");
            errorProperties.Should().Contain("NationalInsuranceNo");
            errorProperties.Should().Contain("StartDate"); // Tenure
            errorProperties.Should().Contain("EndDate"); // Tenure
            errorProperties.Should().Contain("Languages");
            errorProperties.Should().Contain("Name"); // Language
        }

        public void ThenBadRequestIsReturned()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public void ThenNotFoundIsReturned()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
