using AzagraBank.EventBus.Implementations;
using AzagraBank.EventBus.Interfaces;
using AzagraBank.Messages.Commands;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("azagra-bank-cache");

builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddSingleton<IMessagePublisher<DepositCommand>, MessagePublisher<DepositCommand>>();
builder.Services.AddSingleton<IMessagePublisher<WithdrawCommand>, MessagePublisher<WithdrawCommand>>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseOutputCache();

app.UseCors(static builder =>
    builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
