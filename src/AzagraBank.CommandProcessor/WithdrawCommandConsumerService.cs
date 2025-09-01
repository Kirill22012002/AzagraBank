using AzagraBank.CommandProcessor.Exceptions;
using AzagraBank.CommandProcessor.Services.Interfaces;
using AzagraBank.EventBus;
using AzagraBank.EventBus.Interfaces;
using AzagraBank.Messages.Commands;
using AzagraBank.Messages.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzagraBank.CommandProcessor;

public class WithdrawCommandConsumerService : BackgroundService
{
    private readonly ILogger<WithdrawCommandConsumerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageConsumer<WithdrawCommand> _consumer;
    private readonly IMessagePublisher<AccountCreditedEvent> _publisher;

    public WithdrawCommandConsumerService(
        ILogger<WithdrawCommandConsumerService> logger,
        IServiceProvider serviceProvider,
        IMessageConsumer<WithdrawCommand> consumer,
        IMessagePublisher<AccountCreditedEvent> publisher)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _consumer = consumer;
        _publisher = publisher;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Kafka. A processing commands service has been started {nameof(WithdrawCommandConsumerService)}");
        return _consumer.StartAsync(ProcessMessageAsync, CONSTS.KAFKA_COMMANDS_TOPIC, stoppingToken);
    }

    private async Task ProcessMessageAsync(WithdrawCommand message, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var validator = scope.ServiceProvider.GetRequiredService<IWithdrawValidator>();

            var isValid = await validator.ValidateAsync(message.Amount, message.AccountId);
            if (!isValid) throw new TransactionValidationException("not valid");

            var @event = new AccountCreditedEvent
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
