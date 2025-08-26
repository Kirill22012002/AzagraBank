using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace AzagraBank.EventBus;

public class ProducerService : IProducerService
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<ProducerService> _logger;

    public ProducerService(ILogger<ProducerService> logger)
    {
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendMessageAsync(string topic, string message)
    {
        try
        {
            await _producer.ProduceAsync(topic, new Message<string, string> { Value = message });
            _logger.LogInformation("Message {kafka_message} sent to topic {kafka_topic}", message, topic);
        }
        catch(Exception ex)
        {
            _logger.LogError("Error sending message to kafka: {error}", ex.Message);
        }
    }
}
