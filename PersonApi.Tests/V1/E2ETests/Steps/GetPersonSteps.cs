using FluentAssertions;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PersonApi.Tests.V1.E2ETests.Steps
{
    public class GetPersonSteps
    {
        private readonly HttpClient _httpClient;

        private HttpResponseMessage _lastResponse;

        public GetPersonSteps(HttpClient httpClient)
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

        public async Task WhenThePersonDetailsAreRequested(string id)
        {
            var uri = new Uri($"api/v1/persons/{id}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        public async Task ThenThePersonDetailsAreReturned(PersonDbEntity expectedPerson)
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiPerson = JsonSerializer.Deserialize<PersonResponseObject>(responseContent, CreateJsonOptions());

            apiPerson.CommunicationRequirements.Should().BeEquivalentTo(expectedPerson.CommunicationRequirements);
            apiPerson.DateOfBirth.Should().Be(ResponseFactory.FormatDateOfBirth(expectedPerson.DateOfBirth));
            apiPerson.Ethnicity.Should().Be(expectedPerson.Ethnicity);
            apiPerson.Firstname.Should().Be(expectedPerson.Firstname);
            apiPerson.Gender.Should().Be(expectedPerson.Gender);
            apiPerson.Id.Should().Be(expectedPerson.Id);
            apiPerson.Identifications.Should().BeEquivalentTo(expectedPerson.Identifications);
            apiPerson.Languages.Should().BeEquivalentTo(expectedPerson.Languages);
            // TODO - Update the links verification when implemented
            apiPerson.Links.Should().BeEmpty();
            apiPerson.MiddleName.Should().Be(expectedPerson.MiddleName);
            apiPerson.Nationality.Should().Be(expectedPerson.Nationality);
            apiPerson.PersonTypes.Should().BeEquivalentTo(expectedPerson.PersonTypes);
            apiPerson.PlaceOfBirth.Should().Be(expectedPerson.PlaceOfBirth);
            apiPerson.PreferredFirstname.Should().Be(expectedPerson.PreferredFirstname);
            apiPerson.PreferredSurname.Should().Be(expectedPerson.PreferredSurname);
            apiPerson.Surname.Should().Be(expectedPerson.Surname);
            apiPerson.Title.Should().Be(expectedPerson.Title);
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
