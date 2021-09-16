using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Configuration;
using Hackney.Shared.Person;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PersonApi.V1.Gateways
{
    public class PersonSnsGateway : ISnsGateway
    {
        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonOptions;

        public PersonSnsGateway(IAmazonSimpleNotificationService amazonSimpleNotificationService, IConfiguration configuration)
        {
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
            _configuration = configuration;

            _jsonOptions = CreateJsonOptions();
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

        public async Task Publish(PersonSns personSns)
        {
            string message = JsonSerializer.Serialize(personSns, _jsonOptions);
            var request = new PublishRequest
            {
                Message = message,
                TopicArn = Environment.GetEnvironmentVariable("PERSON_SNS_ARN"),
                MessageGroupId = "SomeGroupId"
            };

            await _amazonSimpleNotificationService.PublishAsync(request).ConfigureAwait(false);
        }
    }
}
