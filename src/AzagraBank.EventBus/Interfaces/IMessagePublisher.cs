using AzagraBank.Messages;

namespace AzagraBank.EventBus.Interfaces;

public interface IMessagePublisher<T> where T : IMessage
{
    Task PublishAsync(T message, string topic);
}
