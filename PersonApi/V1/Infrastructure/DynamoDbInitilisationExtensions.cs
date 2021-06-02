using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using Amazon.SimpleNotificationService;

namespace PersonApi.V1.Infrastructure
{
    public static class DynamoDbInitilisationExtensions
    {
        public static void ConfigureAws(this IServiceCollection services)
        {
            bool localMode = false;
            _ = bool.TryParse(Environment.GetEnvironmentVariable("DynamoDb_LocalMode"), out localMode);

            if (localMode)
            {
                var url = Environment.GetEnvironmentVariable("DynamoDb_LocalServiceUrl");
                var snsUrl = Environment.GetEnvironmentVariable("Localstack_SnsServiceUrl");

                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = url };
                    return new AmazonDynamoDBClient(clientConfig);
                });
                services.AddSingleton<IAmazonSimpleNotificationService>(sp =>
                {
                    var clientConfig = new AmazonSimpleNotificationServiceConfig { ServiceURL = snsUrl };
                    return new AmazonSimpleNotificationServiceClient(clientConfig);
                });
            }
            else
            {
                services.AddScoped<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddScoped<IDynamoDBContext>(sp =>
            {
                var db = sp.GetService<IAmazonDynamoDB>();
                return new DynamoDBContext(db);
            });
        }
    }
}
