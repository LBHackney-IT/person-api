using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PersonApi.Tests.V1.E2ETests.Fixtures;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PersonApi.Tests.V1.E2ETests.Steps
{
    public class UpdatePersonStep : BaseSteps
    {
        public UpdatePersonStep(HttpClient httpClient) : base(httpClient)
        { }

        /// <summary>
        /// You can use jwt.io to decode the token - it is the same one we'd use on dev, etc. 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CallApi(UpdatePersonRequestObject requestObject, Guid? id, int? ifMatch)
        {
            var token =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMTUwMTgxMTYwOTIwOTg2NzYxMTMiLCJlbWFpbCI6ImUyZS10ZXN0aW5nQGRldmVsb3BtZW50LmNvbSIsImlzcyI6IkhhY2tuZXkiLCJuYW1lIjoiVGVzdGVyIiwiZ3JvdXBzIjpbImUyZS10ZXN0aW5nIl0sImlhdCI6MTYyMzA1ODIzMn0.SooWAr-NUZLwW8brgiGpi2jZdWjyZBwp4GJikn0PvEw";

            var idString = id.HasValue ? id.Value.ToString() : "dsfoidfjh";
            var uri = new Uri($"api/v1/persons/{idString}", UriKind.Relative);

            var message = new HttpRequestMessage(HttpMethod.Patch, uri);

            var jsonSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new[] { new StringEnumConverter() }
            };
            var requestJson = JsonConvert.SerializeObject(requestObject, jsonSettings);
            message.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Patch;
            message.Headers.Add("Authorization", token);
            message.Headers.TryAddWithoutValidation(HeaderConstants.IfMatch, ifMatch?.ToString());

            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await _httpClient.SendAsync(message).ConfigureAwait(false);
        }

        public async Task WhenTheUpdatePersonApiIsCalled(UpdatePersonRequestObject personRequestObject, Guid? id)
        {
            await WhenTheUpdatePersonApiIsCalled(personRequestObject, id, 0).ConfigureAwait(false);
        }

        public async Task WhenTheUpdatePersonApiIsCalled(UpdatePersonRequestObject personRequestObject, Guid? id, int? ifMatch)
        {
            _lastResponse = await CallApi(personRequestObject, id, ifMatch).ConfigureAwait(false);

        }

        public async Task ThenThePersonDetailsAreUpdated(PersonFixture personFixture)
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var result = await personFixture._dbContext.LoadAsync<PersonDbEntity>(personFixture.Person.Id).ConfigureAwait(false);
            result.FirstName.Should().Be(personFixture.UpdatePersonRequest.FirstName);
            result.Surname.Should().Be(personFixture.UpdatePersonRequest.Surname);
            result.VersionNumber.Should().Be(1);
            result.LastModified.Should().BeCloseTo(DateTime.UtcNow, 500);
        }

        public async Task ThenConflictIsReturned(int? versionNumber)
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var sentVersionNumberString = (versionNumber is null) ? "{null}" : versionNumber.ToString();
            responseContent.Should().Contain($"The version number supplied ({sentVersionNumberString}) does not match the current value on the entity (0).");
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
