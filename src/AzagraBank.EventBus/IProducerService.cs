namespace AzagraBank.EventBus;

public interface IProducerService
{
    Task SendMessageAsync(string topic, string message);
}
