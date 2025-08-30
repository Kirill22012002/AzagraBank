using AzagraBank.CommandProcessor.Exceptions;
using AzagraBank.CommandProcessor.Services;
using AzagraBank.CommandProcessor.Services.Interfaces;
using AzagraBank.EventBus;
using AzagraBank.EventBus.Interfaces;
using AzagraBank.Messages.Commands;
using AzagraBank.Messages.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzagraBank.CommandProcessor;

public class DepositCommandConsumerService : BackgroundService
{
    private readonly ILogger<DepositCommandConsumerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageConsumer<DepositCommand> _consumer;
    private readonly IMessagePublisher<AccountDebitedEvent> _publisher;

    public DepositCommandConsumerService(
        ILogger<DepositCommandConsumerService> logger,
        IServiceProvider serviceProvider,
        IMessageConsumer<DepositCommand> consumer,
        IMessagePublisher<AccountDebitedEvent> publisher)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _consumer = consumer;
        _publisher = publisher;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Kafka. A processing commands service has been started {nameof(DepositCommandConsumerService)}");
        return _consumer.StartAsync(ProcessMessageAsync, stoppingToken, CONSTS.KAFKA_EVENTS_TOPIC);
    }

    private async Task ProcessMessageAsync(DepositCommand message, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IDepositValidator>();

            var isValid = await validator.ValidateAsync(message.Amount, message.AccountId);
            if (!isValid) throw new TransactionValidationException($"Transaction {message.Id} not valid");

            var @event = new AccountDebitedEvent
            {
                Amount = message.Amount
            };

            await _publisher.PublishAsync(@event, CONSTS.KAFKA_EVENTS_TOPIC);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

}
