using AzagraBank.IdentityServer.EFStuff;
using AzagraBank.IdentityServer.MigrationService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ApplicationDbContext>("identityDb");

var host = builder.Build();
host.Run();
