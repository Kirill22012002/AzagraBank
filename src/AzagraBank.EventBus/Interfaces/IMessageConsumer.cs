using AzagraBank.Messages;

namespace AzagraBank.EventBus.Interfaces;

public interface IMessageConsumer<T> where T : IMessage
{
    Task StartAsync(Action<T, CancellationToken> processMessage, CancellationToken stoppingToken, string topic);
}
