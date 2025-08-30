using AzagraBank.EventBus.Interfaces;
using AzagraBank.Messages;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzagraBank.EventBus.Implementations;

public class MessageConsumer<T> : IMessageConsumer<T> where T : IMessage
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<MessageConsumer<T>> _logger;

    public MessageConsumer(ILogger<MessageConsumer<T>> logger)
    {
        _logger = logger;
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }

    public Task StartAsync(Action<T, CancellationToken> processMessage, CancellationToken stoppingToken, string topic)
    {
        _consumer.Subscribe(topic);

        try
        {
            while (true)
            {
                var consumeResult = _consumer.Consume();
                _logger.LogInformation($"Kafka. Consumed message: {consumeResult.Message.Value}");

                processMessage.Invoke(JsonConvert.DeserializeObject<T>(consumeResult.Message.Value), stoppingToken);
            }
        }
        catch (ConsumeException ex)
        {
            _logger.LogError($"Kafka. Error consuming message: {ex.Error.Reason}");
        }
        return Task.CompletedTask;
    }
}
