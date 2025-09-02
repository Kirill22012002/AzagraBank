using AzagraBank.CommandProcessor;
using AzagraBank.CommandProcessor.Services.Implementations;
using AzagraBank.CommandProcessor.Services.Interfaces;
using AzagraBank.EF;
using AzagraBank.EF.Repositories;
using AzagraBank.EventBus;
using AzagraBank.EventBus.Implementations;
using AzagraBank.EventBus.Interfaces;
using AzagraBank.Messages.Commands;
using AzagraBank.Messages.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddKafkaConsumerWithBaseSettings();
builder.AddKafkaProducerWithBaseSettings();

builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddDbContextFactory<AccountDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("azagraBankDb")));

builder.Services.AddTransient<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IWithdrawValidator, WithdrawValidator>();
builder.Services.AddScoped<IDepositValidator, DepositValidator>();

builder.Services.AddSingleton<IMessageConsumer<DepositCommand>, MessageConsumer<DepositCommand>>();
builder.Services.AddSingleton<IMessageConsumer<WithdrawCommand>, MessageConsumer<WithdrawCommand>>();

builder.Services.AddSingleton<IMessagePublisher<AccountCreditedEvent>, MessagePublisher<AccountCreditedEvent>>();
builder.Services.AddSingleton<IMessagePublisher<AccountDebitedEvent>, MessagePublisher<AccountDebitedEvent>>();

builder.Services.AddHostedService<DepositCommandConsumerService>();
builder.Services.AddHostedService<WithdrawCommandConsumerService>();

var host = builder.Build();
host.Run();
