var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("pgsql")
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var azagraBankDb = postgres.AddDatabase("azagra-bank-db");

var azagraBankCache = builder.AddRedis("azagra-bank-cache");

var api = builder.AddProject<Projects.AzagraBank_ApiService>("azagrabank-apiservice")
    .WithReference(azagraBankDb)
    .WaitFor(azagraBankDb)
    .WithReference(azagraBankCache)
    .WaitFor(azagraBankCache)
    .WithExternalHttpEndpoints();

var client = builder.AddNpmApp("react", "../AzagraBank.Client")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
