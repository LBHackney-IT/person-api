using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Strategies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace PersonApi.Tests.PactBroker
{
    public class PactBrokerWebApplicationFactory
        : WebApplicationFactory<TestStartup>
    {
        private readonly List<TableDef> _tables;

        public IAmazonDynamoDB DynamoDb { get; private set; }
        public IDynamoDBContext DynamoDbContext { get; private set; }
        public IAmazonSimpleNotificationService SimpleNotificationService { get; private set; }
        public IAmazonSQS AmazonSQS { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public PactBrokerWebApplicationFactory(List<TableDef> tables)
        {
            _tables = tables;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(b => b.AddEnvironmentVariables())
                .UseStartup<TestStartup>();
            builder.ConfigureServices(services =>
            {
                ConfigureServicesInternal(services);
            });
        }

        private void ConfigureServicesInternal(IServiceCollection services)
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

            services.ConfigureAws();

            var serviceProvider = services.BuildServiceProvider();
            DynamoDb = serviceProvider.GetRequiredService<IAmazonDynamoDB>();
            DynamoDbContext = serviceProvider.GetRequiredService<IDynamoDBContext>();
            SimpleNotificationService = serviceProvider.GetRequiredService<IAmazonSimpleNotificationService>();
            AmazonSQS = new AmazonSQSClient(new AmazonSQSConfig() { ServiceURL = snsUrl });

            Configuration = serviceProvider.GetRequiredService<IConfiguration>();

            AWSXRayRecorder.Instance.ContextMissingStrategy = ContextMissingStrategy.LOG_ERROR;

            DynamoDbTables.EnsureTablesExist(DynamoDb, _tables);
        }
    }
}
