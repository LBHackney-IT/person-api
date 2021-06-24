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

        public UpdatePersonByIdE2ETests(AwsIntegrationTests<Startup> dbFixture)
        {
            _dbFixture = dbFixture;
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
            await SetupTestData(entity).ConfigureAwait(false);
            entity.Surname = "Update";
            var uri = new Uri($"api/v1/persons/{entity.Id}", UriKind.Relative);
            var content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            var response = await _dbFixture.Client.PatchAsync(uri, content).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var dbObject = await _dbFixture.DynamoDbContext.LoadAsync<PersonDbEntity>(entity.Id).ConfigureAwait(false);

            dbObject.Surname.Should().Be(entity.Surname);
        }
    }
}
