using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Polly;
using Polly.Contrib.WaitAndRetry;

// ref
// https://docs.confluent.io/current/clients/dotnet.html

namespace RetrykafkaConsumer
{
    public interface IKafkaConsumer
    {
        void Consume(CancellationToken stoppingToken);
    }

    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ConsumerConfig config;

        public KafkaConsumer(IHttpClientFactory h)
        {
            _httpClientFactory = h;

            config = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest,

                // TODO: set it to false to prevent potential message lost.
                EnableAutoCommit = true
            };
        }

        public void Consume(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe("retry-transient-error");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cts = new CancellationTokenSource();

                    var consumeResult = consumer.Consume(cts.Token);

                    var delay = Backoff.DecorrelatedJitterBackoffV2(
                        medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                        retryCount: 5);

                    var retry = Policy
                        .HandleResult<HttpResponseMessage>(r =>
                            r.StatusCode == HttpStatusCode.InternalServerError)
                        .WaitAndRetryAsync(2,
                        retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),

                        (exception, timeSpan, retryCount, context) =>
                        {
                            Logger.LogThreadInfo($"Retry #{retryCount}");
                        })
                        .ExecuteAsync(callAPI);

                    async Task<HttpResponseMessage> callAPI()
                    {
                        Logger.LogThreadInfo($"Calling API for message " +
                            $"{consumeResult.Message.Key}");

                        var json = new StringContent(
                                consumeResult.Message.Value, Encoding.UTF8,
                                "application/json");

                        var client = _httpClientFactory.CreateClient();

                        using var response = await client.PostAsync(
                            "https://localhost:5001/retry/ConsumerRetryWriteGitHubBranch",
                            json);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Consumed message '{consumeResult.Message.Key}' " +
                                $"at: '{consumeResult.TopicPartitionOffset}'.");
                        }
                        else
                        {
                            Console.WriteLine($"Consumer failed '{consumeResult.Message.Key}' " +
                                $"at: '{consumeResult.TopicPartitionOffset}'.");
                        }

                        return response;
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occured: {e.Error.Reason}");
                }
            }

            consumer.Close();
        }
    }
}
