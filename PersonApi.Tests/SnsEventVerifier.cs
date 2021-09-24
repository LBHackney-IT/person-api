using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PersonApi.Tests
{
    public class SnsEventVerifier<T> where T : class
    {
        private readonly JsonSerializerOptions _jsonOptions;

        private readonly IAmazonSQS _amazonSQS;
        private readonly string _queueUrl;

        public SnsEventVerifier(IAmazonSQS amazonSQS, string queueUrl)
        {
            _amazonSQS = amazonSQS;
            _queueUrl = queueUrl;
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

        public bool VerifySnsEventRaised(Action<T> verifyFunction)
        {
            bool eventFound = false;
            var request = new ReceiveMessageRequest(_queueUrl)
            {
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 2
            };
            var response = _amazonSQS.ReceiveMessageAsync(request).GetAwaiter().GetResult();
            foreach (var msg in response.Messages)
            {
                eventFound = IsExpectedMessage(msg, verifyFunction);
                if (eventFound) break;
            }

            return eventFound;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        private bool IsExpectedMessage(Message msg, Action<T> verifyFunction)
        {
            var payloadString = JsonDocument.Parse(msg.Body).RootElement.GetProperty("Message").GetString();
            var eventObject = JsonSerializer.Deserialize<T>(payloadString, _jsonOptions);
            try
            {
                verifyFunction(eventObject);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
