var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("PostgreSQL")
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var azagraBankDb = postgres.AddDatabase("azagra-bank-db");

var azagraBankCache = builder.AddRedis("Redis");

var kafka = builder.AddKafka("Kafka");

var api = builder.AddProject<Projects.AzagraBank_ApiService>("Api")
    .WithReference(kafka)
    .WaitFor(kafka)
    .WithReference(azagraBankCache)
    .WaitFor(azagraBankCache)
    .WithExternalHttpEndpoints();

var client = builder.AddNpmApp("Client", "../AzagraBank.Client")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(port: 5173, targetPort: 5174, env: "PORT")
    .PublishAsDockerFile();

builder.Build().Run();
