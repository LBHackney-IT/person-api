using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using AutoFixture;
using Bogus.Extensions;
using FluentAssertions;
using Newtonsoft.Json;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Domain;
using PersonApi.V1.Factories;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.E2ETests
{
    [Collection("Aws collection")]
    public class UpdatePersonByIdE2ETests : IDisposable
    {
        private readonly Fixture _fixture = new Fixture();
        public PersonDbEntity Person { get; private set; }
        private readonly AwsIntegrationTests<Startup> _dbFixture;
        private readonly List<Action> _cleanupActions = new List<Action>();
        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;
        public UpdatePersonByIdE2ETests(AwsIntegrationTests<Startup> dbFixture, IAmazonSimpleNotificationService amazonSimpleNotificationService)
        {
            _dbFixture = dbFixture;
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
        }
        private PersonRequestObject ConstructTestEntity()
        {
            var entity = _fixture.Build<PersonRequestObject>()
                                 .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
                                 .With(x => x.NationalInsuranceNo, "NZ223344D")
                                 .With(x => x.Tenures, _fixture.Build<Tenure>()
                                                              .With(y => y.StartDate, "")
                                                              .With(y => y.EndDate, "")
                                                              .CreateMany(2))
                                 .With(x => x.Languages, Enumerable.Empty<Language>())
                                 .Create();
            return entity;
        }

        private void UpdateSnsTopic()
        {
            var snsAttrs = new Dictionary<string, string>();
            snsAttrs.Add("fifo_topic", "true");
            snsAttrs.Add("content_based_deduplication", "true");

            var response = _amazonSimpleNotificationService.CreateTopicAsync(new CreateTopicRequest
            {
                Name = "personupdated",
                Attributes = snsAttrs
            }).Result;

            Environment.SetEnvironmentVariable("UPDATED_PERSON_SNS_ARN", response.TopicArn);
        }

        /// <summary>
        /// Method to add an entity instance to the database so that it can be used in a test.
        /// Also adds the corresponding action to remove the upserted data from the database when the test is done.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task SetupTestData(PersonRequestObject entity)
        {
            await _dbFixture.DynamoDbContext.SaveAsync(entity.ToDatabase()).ConfigureAwait(false);
            _cleanupActions.Add(async () => await _dbFixture.DynamoDbContext.DeleteAsync<PersonDbEntity>(entity.Id).ConfigureAwait(false));
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                foreach (var action in _cleanupActions)
                    action();

                _disposed = true;
            }
        }

        [Fact]
        public async Task UpdatePersonByIdNotFoundReturns404()
        {
            var entity = ConstructTestEntity();
            var uri = new Uri($"api/v1/persons/{entity.Id}", UriKind.Relative);
            var content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            var response = await _dbFixture.Client.PatchAsync(uri, content).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdatedPersonByIdFoundSuccessfullyUpdates()
        {
            var entity = ConstructTestEntity();
            UpdateSnsTopic();
            var token =
                "eyJhbGciOiJIUzM4NCIsInR5cCI6IkpXVCJ9.eyJncm91cHMiOiJlMmUtdGVzdGluZy1kZXZlbG9wbWVudCIsImVtYWlsIjoiZTJlLXRlc3RpbmctZGV2ZWxvcG1lbnRAaGFja25leS5nb3YudWsiLCJuYW1lIjoiZTJlLXRlc3RpbmctZGV2ZWxvcG1lbnQiLCJuYmYiOjE2MjIwMTk4NTgsImV4cCI6MTkzNzU1MjY1OCwiaWF0IjoxNjIyMDE5ODU4fQ.SoUUGRHkHxSqEfS0gXu2CT_lZtK2IwKLEJc2QfKWA4qGq9LmjnGbanM-5H-J9Xz-";
            await SetupTestData(entity).ConfigureAwait(false);
            entity.Surname = "Update";
            var uri = new Uri($"api/v1/persons/{entity.Id}", UriKind.Relative);
            var message = new HttpRequestMessage(HttpMethod.Patch, uri);
            message.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Patch;
            message.Headers.Add("Authorization", token);
            //var content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            //var response = await _dbFixture.Client.PatchAsync(uri, content).ConfigureAwait(false);

            _dbFixture.Client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _dbFixture.Client.SendAsync(message).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var dbObject = await _dbFixture.DynamoDbContext.LoadAsync<PersonDbEntity>(entity.Id).ConfigureAwait(false);

            dbObject.Surname.Should().Be(entity.Surname);
        }
    }
}
