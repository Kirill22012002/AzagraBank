using AzagraBank.CommandProcessor;
using AzagraBank.CommandProcessor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
