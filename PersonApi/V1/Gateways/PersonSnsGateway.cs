using System;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
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

        public PersonSnsGateway(IAmazonSimpleNotificationService amazonSimpleNotificationService, IOptions<AwsConfiguration> settings)
        {
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
            _settings = settings;
        }

        [LogCall]
        public async Task Publish(PersonSns personSns)
        {
            string message = JsonConvert.SerializeObject(personSns);

            var request = new PublishRequest { Message = message, TopicArn = Environment.GetEnvironmentVariable("PersonTopicArn") };
            await _amazonSimpleNotificationService.PublishAsync(request).ConfigureAwait(false);
        }
    }
}
