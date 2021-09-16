using FluentAssertions;
using PersonApi.V1.Boundary.Response;
using Hackney.Shared.Person;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersonApi.Tests.V1.E2ETests.Steps
{
    public class GetPersonSteps : BaseSteps
    {
        public GetPersonSteps(HttpClient httpClient) : base(httpClient)
        { }

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

            var eTagHeaders = _lastResponse.Headers.GetValues(HeaderConstants.ETag);
            eTagHeaders.Count().Should().Be(1);
            eTagHeaders.First().Should().Be($"\"{expectedPerson.VersionNumber.ToString()}\"");

            apiPerson.DateOfBirth.Should().Be(ResponseFactory.FormatDateOfBirth(expectedPerson.DateOfBirth));
            apiPerson.FirstName.Should().Be(expectedPerson.FirstName);
            apiPerson.Id.Should().Be(expectedPerson.Id);
            // TODO - Update the links verification when implemented
            apiPerson.Links.Should().BeEmpty();
            apiPerson.MiddleName.Should().Be(expectedPerson.MiddleName);
            apiPerson.PersonTypes.Should().BeEquivalentTo(expectedPerson.PersonTypes);
            apiPerson.PlaceOfBirth.Should().Be(expectedPerson.PlaceOfBirth);
            apiPerson.PreferredFirstName.Should().Be(expectedPerson.PreferredFirstName);
            apiPerson.PreferredSurname.Should().Be(expectedPerson.PreferredSurname);
            apiPerson.Surname.Should().Be(expectedPerson.Surname);
            apiPerson.Title.Should().Be(expectedPerson.Title);
            apiPerson.Tenures.Should().BeEquivalentTo(expectedPerson.Tenures?.Select(x => ResponseFactory.ToResponse(x)));
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
