using System;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PersonApi.V1.Domain;
using PersonApi.V1.Domain.Configuration;
using PersonApi.V1.Logging;

namespace PersonApi.V1.Gateways
{
    public class PersonSnsGateway : ISnsGateway
    {
        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;
        private readonly IOptions<AwsConfiguration> _settings;
        private readonly ILogger<PersonSnsGateway> _logger;

        public PersonSnsGateway(IAmazonSimpleNotificationService amazonSimpleNotificationService, IOptions<AwsConfiguration> settings, ILogger<PersonSnsGateway> logger)
        {
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
            _settings = settings;
            _logger = logger;
        }

        [LogCall]
        public async Task Publish(PersonSns personSns)
        {
            string message = JsonConvert.SerializeObject(personSns);
            _logger.Log(LogLevel.Information, Environment.GetEnvironmentVariable("DynamoDb_LocalMode"));
            _logger.Log(LogLevel.Information, Environment.GetEnvironmentVariable("Localstack_SnsServiceUrl"));

            var request = new PublishRequest { Message = message, TopicArn = Environment.GetEnvironmentVariable("PersonTopicArn") };
            await _amazonSimpleNotificationService.PublishAsync(request).ConfigureAwait(false);
        }
    }
}
