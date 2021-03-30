using FluentAssertions;
using Newtonsoft.Json;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Infrastructure;
using System;
using System.Net;
using System.Net.Http;
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

        public async Task WhenThePersonDetailsAreRequested(string id)
        {
            var uri = new Uri($"api/v1/persons/{id}", UriKind.Relative);
            _lastResponse = await _httpClient.GetAsync(uri).ConfigureAwait(false);
        }

        public async Task ThenThePersonDetailsAreReturned(PersonDbEntity expectedPerson)
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await _lastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiPerson = JsonConvert.DeserializeObject<PersonResponseObject>(responseContent);

            apiPerson.CommunicationRequirements.Should().BeEquivalentTo(expectedPerson.CommunicationRequirements);
            apiPerson.DateOfBirth.Should().Be(expectedPerson.DateOfBirth);
            apiPerson.Ethinicity.Should().Be(expectedPerson.Ethinicity);
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
