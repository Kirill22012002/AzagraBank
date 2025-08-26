namespace AzagraBank.CommandProcessor.Services;

public interface IConsumerService
{
    void ConsumeMessages(string topic);
}
