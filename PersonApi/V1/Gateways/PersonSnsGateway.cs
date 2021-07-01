using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PersonApi.V1.Domain;

namespace PersonApi.V1.Gateways
{
    public class PersonSnsGateway : ISnsGateway
    {
        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;
        private readonly IConfiguration _configuration;

        public PersonSnsGateway(IAmazonSimpleNotificationService amazonSimpleNotificationService, IConfiguration configuration)
        {
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
            _configuration = configuration;
        }

        public async Task Publish(PersonSns personSns)
        {
            string message = JsonConvert.SerializeObject(personSns);
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
