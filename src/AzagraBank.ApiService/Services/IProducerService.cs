namespace AzagraBank.ApiService.Services;

public interface IProducerService
{
    Task SendMessageAsync(string topic, string message);
}
