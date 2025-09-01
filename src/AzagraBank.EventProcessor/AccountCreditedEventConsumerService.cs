using AzagraBank.EventBus;
using AzagraBank.EventBus.Interfaces;
using AzagraBank.Messages.Events;

namespace AzagraBank.EventProcessor;

public class AccountCreditedEventConsumerService : BackgroundService
{
    private readonly ILogger<AccountCreditedEventConsumerService> _logger;
    private readonly IMessageConsumer<AccountCreditedEvent> _consumer;

    public AccountCreditedEventConsumerService(
        ILogger<AccountCreditedEventConsumerService> logger,
        IMessageConsumer<AccountCreditedEvent> consumer)
    {
        _logger = logger;
        _consumer = consumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Kafka. A processing events service has been started {nameof(AccountCreditedEventConsumerService)}");
        return _consumer.StartAsync(ProcessMessageAsync, CONSTS.KAFKA_EVENTS_TOPIC, stoppingToken);
    }

    private Task ProcessMessageAsync(AccountCreditedEvent message, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"WE NEED TO SAVE EVENT: {message.Id} to event log");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        return Task.CompletedTask;
    }
}
