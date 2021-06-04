using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Xunit;

namespace PersonApi.Tests
{
    public class AwsIntegrationTests<TStartup> : IDisposable where TStartup : class
    {
        public HttpClient Client { get; private set; }
        public IDynamoDBContext DynamoDbContext => _factory?.DynamoDbContext;
        public IAmazonSimpleNotificationService SimpleNotificationService => _factory?.SimpleNotificationService;

        private readonly AwsMockWebApplicationFactory<TStartup> _factory;
        private readonly List<TableDef> _tables = new List<TableDef>
        {
            new TableDef { Name = "Persons", KeyName = "id", KeyType = ScalarAttributeType.S }
        };

        public AwsIntegrationTests()
        {
            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            EnsureEnvVarConfigured("Localstack_SnsServiceUrl", "http://localhost:4566");

            _factory = new AwsMockWebApplicationFactory<TStartup>(_tables);
            Client = _factory.CreateClient();

            CreateSnsTopic();
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
                if (null != _factory)
                    _factory.Dispose();
                _disposed = true;
            }
        }

        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                Environment.SetEnvironmentVariable(name, defaultValue);
        }

        private void CreateSnsTopic()
        {
            var snsAttrs = new Dictionary<string, string>();
            snsAttrs.Add("fifo_topic", "true");
            snsAttrs.Add("content_based_deduplication", "true");

            var response = SimpleNotificationService.CreateTopicAsync(new CreateTopicRequest
            {
                Name = "personcreated",
                Attributes = snsAttrs
            }).Result;

            Environment.SetEnvironmentVariable("NEW_PERSON_SNS_ARN", response.TopicArn);
        }
    }

    public class TableDef
    {
        public string Name { get; set; }
        public string KeyName { get; set; }
        public ScalarAttributeType KeyType { get; set; }
    }


    [CollectionDefinition("Aws collection", DisableParallelization = true)]
    public class DynamoDbCollection : ICollectionFixture<AwsIntegrationTests<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
