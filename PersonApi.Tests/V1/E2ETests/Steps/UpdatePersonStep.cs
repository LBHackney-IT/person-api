using Amazon.SimpleNotificationService.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonApi.Tests.V1.E2ETests.Fixtures;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<HttpResponseMessage> CallApi(UpdatePersonRequestObject requestObject, Guid id)
        {
            var token =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMTUwMTgxMTYwOTIwOTg2NzYxMTMiLCJlbWFpbCI6ImUyZS10ZXN0aW5nQGRldmVsb3BtZW50LmNvbSIsImlzcyI6IkhhY2tuZXkiLCJuYW1lIjoiVGVzdGVyIiwiZ3JvdXBzIjpbImUyZS10ZXN0aW5nIl0sImlhdCI6MTYyMzA1ODIzMn0.SooWAr-NUZLwW8brgiGpi2jZdWjyZBwp4GJikn0PvEw";

            var uri = new Uri($"api/v1/persons/{id}", UriKind.Relative);

            var message = new HttpRequestMessage(HttpMethod.Patch, uri);

            message.Content = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Patch;
            message.Headers.Add("Authorization", token);

            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await _httpClient.SendAsync(message).ConfigureAwait(false);
        }

        public async Task WhenTheUpdatePersonApiIsCalled(UpdatePersonRequestObject personRequestObject, Guid id)
        {
            _lastResponse = await CallApi(personRequestObject, id).ConfigureAwait(false);

        }

        public async Task ThenThePersonDetailsAreUpdated(PersonFixture personFixture)
        {
            _lastResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var result = await personFixture._dbContext.LoadAsync<PersonDbEntity>(personFixture.Person.Id).ConfigureAwait(false);
            result.Surname.Should().Be(personFixture.UpdatePersonRequest.Surname);
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
