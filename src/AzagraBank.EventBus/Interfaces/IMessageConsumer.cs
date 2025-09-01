using AzagraBank.Messages;

namespace AzagraBank.EventBus.Interfaces;

public delegate Task ProcessMessage<T>(T message, CancellationToken stoppingToken) where T : IMessage;

public interface IMessageConsumer<T> where T : IMessage
{
    Task StartAsync(ProcessMessage<T> processMessage, string topic, CancellationToken stoppingToken);
}
