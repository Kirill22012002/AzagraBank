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

    public MessageConsumer(
        ILogger<MessageConsumer<T>> logger, 
        IConsumer<string, string> consumer)
    {
        _logger = logger;
        _consumer = consumer;
    }

    public Task StartAsync(ProcessMessage<T> processMessage, string topic, CancellationToken stoppingToken)
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
