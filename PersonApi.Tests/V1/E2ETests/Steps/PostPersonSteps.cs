using FluentAssertions;
using PersonApi.V1.Boundary.Response;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PersonApi.V1.Boundary.Request;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PersonApi.Tests.V1.E2ETests.Steps
{
    public class PostPersonSteps
    {
        private readonly HttpClient _httpClient;

        private HttpResponseMessage _lastResponse;

        public PostPersonSteps(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private static JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        public async Task WhenAPersonIsCreated(PersonRequestObject requestObject)
        {
            var uri = new Uri($"api/v1/persons", UriKind.Relative);
            var message = new HttpRequestMessage(HttpMethod.Post, uri);
            message.Content = new StringContent(JsonConvert.SerializeObject(requestObject));
            message.Method = HttpMethod.Post;
            message.Headers.Authorization = new AuthenticationHeaderValue("OAuth2", "eyJhbGciOiJIUzM4NCIsInR5cCI6IkpXVCJ9.eyJncm91cHMiOiJlMmUtdGVzdGluZy1kZXZlbG9wbWVudCIsImVtYWlsIjoiZTJlLXRlc3RpbmctZGV2ZWxvcG1lbnRAaGFja25leS5nb3YudWsiLCJuYW1lIjoiZTJlLXRlc3RpbmctZGV2ZWxvcG1lbnQiLCJuYmYiOjE2MjIwMTk4NTgsImV4cCI6MTkzNzU1MjY1OCwiaWF0IjoxNjIyMDE5ODU4fQ.SoUUGRHkHxSqEfS0gXu2CT_lZtK2IwKLEJc2QfKWA4qGq9LmjnGbanM-5H-J9Xz-");

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
