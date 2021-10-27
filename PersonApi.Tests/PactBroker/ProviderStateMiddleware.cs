using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using Hackney.Shared.Person.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersonApi.Tests.PactBroker
{
    public class ProviderStateMiddleware
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly IDictionary<string, Action<string, IDictionary<string, string>>> _providerStates;
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _services;
        private readonly Fixture _fixture = new Fixture();
        private static readonly List<Action> _cleanup = new List<Action>();

        public ProviderStateMiddleware(RequestDelegate next, IServiceProvider services)
        {
            _next = next;
            _services = services;

            _providerStates = new Dictionary<string, Action<string, IDictionary<string, string>>>
            {
                {
                    "the Person API has a person with an id:6fbe024f-2316-4265-a6e8-d65a837e308a",
                    ARecordExists
                }
            };
        }

        private void ARecordExists(string name, IDictionary<string, string> parameters)
        {
            string id = null;
            if (parameters.ContainsKey("id"))
                id = parameters["id"];
            else
                id = name.Split(':')?.Last();

            if (!string.IsNullOrEmpty(id))
            {
                var testPerson = _fixture.Build<PersonDbEntity>()
                                         .With(x => x.Id, Guid.Parse(id))
                                         .With(x => x.VersionNumber, (int?) null)
                                         .Create();
                var dynamoDbContext = _services.GetRequiredService<IDynamoDBContext>();
                dynamoDbContext.SaveAsync<PersonDbEntity>(testPerson).GetAwaiter().GetResult();

                _cleanup.Add(async () => await dynamoDbContext.DeleteAsync<PersonDbEntity>(testPerson).ConfigureAwait(false));
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!(context.Request.Path.Value?.StartsWith("/provider-states") ?? false))
            {
                await _next.Invoke(context).ConfigureAwait(false);
                return;
            }

            context.Response.StatusCode = (int) HttpStatusCode.OK;

            if (context.Request.Method == HttpMethod.Post.ToString())
            {
                string jsonRequestBody;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync().ConfigureAwait(false);
                }

                var providerState = JsonSerializer.Deserialize<ProviderState>(jsonRequestBody, _options);

                //A null or empty provider state key must be handled
                if (!string.IsNullOrEmpty(providerState?.State))
                {
                    if (_providerStates.ContainsKey(providerState.State))
                    {
                        _providerStates[providerState.State].Invoke(providerState.State, providerState.Params);
                        await context.Response.WriteAsync($"Executed actions for provider state step: {providerState.State}")
                                     .ConfigureAwait(false);
                    }
                    else
                        await context.Response.WriteAsync($"No actions configured for provider state step: {providerState.State}")
                                              .ConfigureAwait(false);
                }
            }
        }

        public static void Cleanup()
        {
            foreach (var act in _cleanup)
                act();
            _cleanup.Clear();
        }
    }
}
