namespace AzagraBank.EventBus;

public interface IConsumerService
{
    Task ConsumeMessagesAsync(string topic);
}
