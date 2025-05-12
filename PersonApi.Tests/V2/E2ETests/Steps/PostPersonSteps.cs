using FluentAssertions;
using Hackney.Core.Testing.Sns;
using Hackney.Shared.Person;
using Hackney.Shared.Person.Boundary.Request;
using Hackney.Shared.Person.Boundary.Request.Validation;
using Hackney.Shared.Person.Boundary.Response;
using Hackney.Shared.Person.Factories;
using Hackney.Shared.Person.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonApi.Tests.V1.E2ETests.Fixtures;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PersonApi.Tests.V2.E2ETests.Steps
{
    public class PostPersonSteps : BaseSteps
    {
        public PostPersonSteps(HttpClient httpClient) : base(httpClient)
        { }

        private static void ShouldHaveErrorFor(JEnumerable<JToken> errors, string propertyName, string errorCode = null)
        {
            var error = errors.FirstOrDefault(x => (x.Path.Split('.').Last().Trim('\'', ']')) == propertyName) as JProperty;
            error.Should().NotBeNull();
            if (!string.IsNullOrEmpty(errorCode))
                error.Value.ToString().Should().Contain(errorCode);
        }

        /// <summary>
        /// You can use jwt.io to decode the token - it is the same one we'd use on dev, etc. 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task WhenTheCreatePersonApiIsCalled(CreatePersonRequestObject requestObject)
        {
            var token =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMTUwMTgxMTYwOTIwOTg2NzYxMTMiLCJlbWFpbCI6ImUyZS10ZXN0aW5nQGRldmVsb3BtZW50LmNvbSIsImlzcyI6IkhhY2tuZXkiLCJuYW1lIjoiVGVzdGVyIiwiZ3JvdXBzIjpbImUyZS10ZXN0aW5nIl0sImlhdCI6MTYyMzA1ODIzMn0.SooWAr-NUZLwW8brgiGpi2jZdWjyZBwp4GJikn0PvEw";
            var uri = new Uri($"api/v2/persons", UriKind.Relative);

            var message = new HttpRequestMessage(HttpMethod.Post, uri);

            message.Content = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Post;
            message.Headers.Add("Authorization", token);

            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _lastResponse = await _httpClient.SendAsync(message).ConfigureAwait(false);
        }

        public async Task ThenThePersonCreatedEventIsRaised(ISnsFixture snsFixture)
        {
            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiPerson = JsonSerializer.Deserialize<PersonResponseObject>(responseContent, CreateJsonOptions());

            Action<PersonSns> verifyFunc = (actual) =>
            {
                actual.CorrelationId.Should().NotBeEmpty();
                actual.DateTime.Should().BeCloseTo(DateTime.UtcNow, 1000);
                actual.EntityId.Should().Be(apiPerson.Id);
                var newData = JsonSerializer.Deserialize<Person>(responseContent, CreateJsonOptions());
                var newDataAsResponse = new ResponseFactory(null).ToResponse(newData);
                newDataAsResponse.Should().BeEquivalentTo(apiPerson, c => c.Excluding(y => y.Links));
                actual.EventType.Should().Be(PersonApi.V2.Infrastructure.CreateEventConstants.EVENTTYPE);
                actual.Id.Should().NotBeEmpty();
                actual.SourceDomain.Should().Be(PersonApi.V2.Infrastructure.CreateEventConstants.SOURCEDOMAIN);
                actual.SourceSystem.Should().Be(PersonApi.V2.Infrastructure.CreateEventConstants.SOURCESYSTEM);
                actual.User.Email.Should().Be("e2e-testing@development.com");
                actual.User.Name.Should().Be("Tester");
                actual.Version.Should().Be(PersonApi.V2.Infrastructure.CreateEventConstants.V2VERSION);
            };

            var snsVerifer = snsFixture.GetSnsEventVerifier<PersonSns>();
            var snsResult = await snsVerifer.VerifySnsEventRaised(verifyFunc);
            if (!snsResult && snsVerifer.LastException != null)
                throw snsVerifer.LastException;
        }

        public async Task ThenThePersonDetailsAreReturnedAndIdIsNotEmpty(PersonFixture personFixture)
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiPerson = JsonSerializer.Deserialize<PersonResponseObject>(responseContent, CreateJsonOptions());

            apiPerson.Id.Should().NotBeEmpty();

            var dbRecord = await personFixture._dbContext.LoadAsync<PersonDbEntity>(apiPerson.Id).ConfigureAwait(false);
            apiPerson.Should().BeEquivalentTo(new ResponseFactory(null).ToResponse(dbRecord.ToDomain()),
                                              config => config.Excluding(y => y.Links));
            dbRecord.VersionNumber.Should().Be(0);
            dbRecord.LastModified.Should().BeCloseTo(DateTime.UtcNow, 1500);

            await personFixture._dbContext.DeleteAsync<PersonDbEntity>(dbRecord.Id).ConfigureAwait(false);
        }

        public async Task ThenPersonRefIsUpdatedInRefGeneratorTable(PersonFixture personFixture, int personRef)
        {
            var newRefGenerator = await personFixture._dbContext.LoadAsync<RefGeneratorEntity>("personRef").ConfigureAwait(false);
            newRefGenerator.RefValue.Should().Be(personRef + 1);
        }

        public async Task ThenTheValidationErrorsAreReturned()
        {
            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            JObject jo = JObject.Parse(responseContent);
            var errors = jo["errors"].Children();

            ShouldHaveErrorFor(errors, "FirstName", ErrorCodes.FirstNameMandatory);
            ShouldHaveErrorFor(errors, "Surname", ErrorCodes.SurnameMandatory);
            ShouldHaveErrorFor(errors, "PersonTypes", ErrorCodes.PersonTypeMandatory);
            ShouldHaveErrorFor(errors, "DateOfBirth", ErrorCodes.DoBInFuture);
            ShouldHaveErrorFor(errors, "StartDate");
            ShouldHaveErrorFor(errors, "EndDate");
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
