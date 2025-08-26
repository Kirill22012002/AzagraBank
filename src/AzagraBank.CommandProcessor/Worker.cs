using AzagraBank.CommandProcessor.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzagraBank.CommandProcessor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConsumerService _consumerService;

    public Worker(ILogger<Worker> logger, IConsumerService consumerService)
    {
        _logger = logger;
        _consumerService = consumerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Start {name_of_service}", nameof(Worker));
        _consumerService.ConsumeMessages(topic: "commands");
    }
}
