using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace AzagraBank.CommandProcessor.Services;

public class ConsumerService : IConsumerService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<ConsumerService> _logger;

    public ConsumerService(ILogger<ConsumerService> logger)
    {
        _logger = logger;
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "commands-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }

    public void ConsumeMessages(string topic)
    {
        _consumer.Subscribe(topic);
        try
        {
            while (true)
            {
                var consumeResult = _consumer.Consume();
                _logger.LogInformation("Consumed message: {kafka_message}", consumeResult.Message.Value);
            }
        }
        catch(ConsumeException ex)
        {
            _logger.LogError("Error consuming message: {error}", ex.Error.Reason);
        }
    }
}
