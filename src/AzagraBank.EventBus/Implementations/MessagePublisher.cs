using AzagraBank.EventBus.Interfaces;
using AzagraBank.Messages;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzagraBank.EventBus.Implementations;

public class MessagePublisher<T> : IMessagePublisher<T> where T : IMessage
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<MessagePublisher<T>> _logger;

    public MessagePublisher(ILogger<MessagePublisher<T>> logger)
    {
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync(T message, string topic)
    {
        try
        {
            await _producer.ProduceAsync(
                topic,
                new Message<string, string> { Value = JsonConvert.SerializeObject(message) });

            _logger.LogInformation("Message {kafka_message} sent to topic {kafka_topic}", message, topic);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error sending message to kafka: {error}", ex.Message);
        }
    }
}
