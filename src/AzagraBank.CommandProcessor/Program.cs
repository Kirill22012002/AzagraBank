using AzagraBank.CommandProcessor;
using AzagraBank.CommandProcessor.Services;
using AzagraBank.EF;
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

builder.AddNpgsqlDbContext<AccountDbContext>("azagra-bank-db");

builder.Services.AddTransient<IAccountRepository, AccountRepository>();

builder.Services.AddTransient<IWithdrawValidator, WithdrawValidator>();
builder.Services.AddTransient<IDepositValidator, DepositValidator>();

builder.Services.AddTransient<ICommandProcessor, CommandProcessor>();

builder.Services.AddSingleton<IProducerService, ProducerService>();
builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
