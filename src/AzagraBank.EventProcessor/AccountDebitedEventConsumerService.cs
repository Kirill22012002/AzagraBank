using AzagraBank.EventBus;
using AzagraBank.EventBus.Interfaces;
using AzagraBank.Messages.Events;

namespace AzagraBank.EventProcessor;

public class AccountDebitedEventConsumerService : BackgroundService
{
    private readonly ILogger<AccountDebitedEventConsumerService> _logger;
    private readonly IMessageConsumer<AccountDebitedEvent> _consumer;

    public AccountDebitedEventConsumerService(
        ILogger<AccountDebitedEventConsumerService> logger,
        IMessageConsumer<AccountDebitedEvent> consumer)
    {
        _logger = logger;
        _consumer = consumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Kafka. A processing events service has been started {nameof(AccountDebitedEventConsumerService)}");
        return _consumer.StartAsync(ProcessMessageAsync, CONSTS.KAFKA_EVENTS_TOPIC, stoppingToken);
    }

    private Task ProcessMessageAsync(AccountDebitedEvent message, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"WE NEED TO SAVE EVENT: {message.Id} to event log");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        return Task.CompletedTask;
    }
}
