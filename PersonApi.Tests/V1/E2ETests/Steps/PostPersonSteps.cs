using FluentAssertions;
using Newtonsoft.Json;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PersonApi.Tests.V1.E2ETests.Steps
{
    public class PostPersonSteps : BaseSteps
    {
        public PostPersonSteps(HttpClient httpClient) : base(httpClient)
        { }

        public async Task WhenAPersonIsCreated(PersonRequestObject requestObject)
        {
            var uri = new Uri($"api/v1/persons", UriKind.Relative);
            HttpContent content = new StringContent(JsonConvert.SerializeObject(requestObject));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            _lastResponse = await _httpClient.PostAsync(uri, content).ConfigureAwait(false);
        }

        public async Task ThenThePersonDetailsAreReturnedAndIdIsNotEmpty()
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiPerson = JsonSerializer.Deserialize<PersonResponseObject>(responseContent, CreateJsonOptions());

            apiPerson.Id.Should().NotBeEmpty();
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
