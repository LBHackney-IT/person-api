using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PersonApi.V1.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class AwsInitilisationExtensions
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
                services.TryAddSingleton<IAmazonSimpleNotificationService>(sp =>
                {
                    var clientConfig = new AmazonSimpleNotificationServiceConfig { ServiceURL = snsUrl };
                    return new AmazonSimpleNotificationServiceClient(clientConfig);
                });
            }
            else
            {
                services.TryAddScoped<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
                services.TryAddAWSService<IAmazonDynamoDB>();
            }

            services.AddScoped<IDynamoDBContext>(sp =>
            {
                var db = sp.GetService<IAmazonDynamoDB>();
                return new DynamoDBContext(db);
            });
        }
    }
}
