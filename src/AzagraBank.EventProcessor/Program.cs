using AzagraBank.EventBus;
using AzagraBank.EventProcessor;
using AzagraBank.EventProcessor.Services;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddSingleton<IConsumerService, ConsumerService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
