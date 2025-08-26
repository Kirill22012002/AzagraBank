using AzagraBank.EventBus;
using AzagraBank.Messages;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzagraBank.CommandProcessor.Services;

public class ConsumerService : IConsumerService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<ConsumerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IProducerService _producerService;

    public ConsumerService(
        ILogger<ConsumerService> logger,
        IServiceProvider serviceProvider,
        IProducerService producerService)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _producerService = producerService;
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "commands-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }

    public async Task ConsumeMessagesAsync(string topic)
    {
        _consumer.Subscribe(topic);
        try
        {
            while (true)
            {
                var consumeResult = _consumer.Consume();
                _logger.LogInformation("Consumed message: {kafka_message}", consumeResult.Message.Value);

                using (var scope = _serviceProvider.CreateScope()) 
                {
                    var commandProcessor = scope.ServiceProvider.GetRequiredService<ICommandProcessor>();
                    var command = JsonConvert.DeserializeObject<Command>(consumeResult.Message.Value);
                    var @event = await commandProcessor.ProcessCommandAsync(command);
                    await _producerService.SendMessageAsync("events", JsonConvert.SerializeObject(@event));
                }
            }
        }
        catch(ConsumeException ex)
        {
            _logger.LogError("Error consuming message: {error}", ex.Error.Reason);
        }
    }
}
