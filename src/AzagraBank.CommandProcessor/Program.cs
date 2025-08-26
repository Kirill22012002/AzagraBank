using AzagraBank.CommandProcessor;
using AzagraBank.CommandProcessor.Services;
using AzagraBank.EF.Repositories;
using AzagraBank.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddScoped<ITransactionValidator, WithdrawValidator>();
builder.Services.AddScoped<ITransactionValidator, DepositValidator>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.Services.AddSingleton<IProducerService, ProducerService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
