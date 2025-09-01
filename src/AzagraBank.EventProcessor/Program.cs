using AzagraBank.EventBus;
using AzagraBank.EventBus.Implementations;
using AzagraBank.EventBus.Interfaces;
using AzagraBank.EventProcessor;
using AzagraBank.Messages.Events;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.AddKafkaConsumerWithBaseSettings();

builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddSingleton<IMessageConsumer<AccountDebitedEvent>, MessageConsumer<AccountDebitedEvent>>();
builder.Services.AddSingleton<IMessageConsumer<AccountCreditedEvent>, MessageConsumer<AccountCreditedEvent>>();

builder.Services.AddHostedService<AccountDebitedEventConsumerService>();
builder.Services.AddHostedService<AccountCreditedEventConsumerService>();

var host = builder.Build();
host.Run();
