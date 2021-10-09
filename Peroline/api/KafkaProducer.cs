using System;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace api
{
    // ref:
    // https://docs.confluent.io/current/clients/dotnet.html
    // https://github.com/confluentinc/confluent-kafka-dotnet

    public interface IKafkaProducer
    {
        void Produce<T>(string msgKey, T msgVal, string topic = "retry-transient-error");
    }

    public class KafkaProducer : IKafkaProducer
    {
        private readonly ClientConfig config;

        public KafkaProducer()
        {
            config = new ClientConfig
            {
                BootstrapServers = "localhost:9092",
            };
        }

        public void Produce<T>(string msgKey, T msgVal, string topic = "retry-transient-error")
        {
            Logger.LogThreadInfo("Produce() method");

            using var producer = new ProducerBuilder<string, string>(config).Build();

            var val = JsonConvert.SerializeObject(msgVal);

            Logger.LogInfo($"Producing record: {val}");

            producer.Produce(
                topic,

                new Message<string, string>
                { Key = msgKey, Value = val },

                Handler
            );

            producer.Flush(TimeSpan.FromSeconds(10));

            Console.WriteLine($"{msgKey} message produced to topic {topic}");
        }

        private void Handler(DeliveryReport<string, string> deliveryReport)
        {

        }
    }
}
