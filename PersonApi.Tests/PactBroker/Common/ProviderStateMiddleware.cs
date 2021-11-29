using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hackney.Core.Testing.PactBroker
{
    public class ProviderStateMiddleware
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        private static IPactBrokerHandler _pactBrokerHandler;
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _services;

        public ProviderStateMiddleware(RequestDelegate next, IServiceProvider services)
        {
            _next = next;
            _services = services;
        }

        private IPactBrokerHandler GetHandler()
        {
            _pactBrokerHandler = _pactBrokerHandler ?? _services.GetRequiredService<IPactBrokerHandler>();
            return _pactBrokerHandler;
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
                    var handler = GetHandler();
                    if (handler.ProviderStates.ContainsKey(providerState.State))
                    {
                        handler.ProviderStates[providerState.State].Invoke(providerState.State, providerState.Params);
                        await context.Response.WriteAsync($"Executed actions for provider state step: {providerState.State}")
                                     .ConfigureAwait(false);
                    }
                    else
                        await context.Response.WriteAsync($"No actions configured for provider state step: {providerState.State}")
                                              .ConfigureAwait(false);
                }
            }
        }
    }
}
