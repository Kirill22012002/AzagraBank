var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgresPassword");

var postgres = builder
    .AddPostgres(name: "postgres", password: postgresPassword)
    .WithDataVolume()
    .WithPgWeb()
    .WithLifetime(ContainerLifetime.Persistent);

var azagraBankDb = postgres.AddDatabase("postgresdb");

var azagraBankCache = builder.AddRedis("Redis");

var kafka = builder.AddKafka("kafka")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithKafkaUI();

var api = builder.AddProject<Projects.AzagraBank_ApiService>("Api")
    .WithReference(kafka).WaitFor(kafka)
    .WithReference(azagraBankCache).WaitFor(azagraBankCache)
    .WithExternalHttpEndpoints();

var commandsProcessor = builder.AddProject<Projects.AzagraBank_CommandProcessor>("CommandsProcessor")
    .WithReference(kafka).WaitFor(kafka)
    .WithReference(postgres).WaitFor(postgres);

var eventsProcessor = builder.AddProject<Projects.AzagraBank_EventProcessor>("EventsProcessor")
    .WithReference(kafka).WaitFor(kafka);

var client = builder.AddNpmApp("Client", "../AzagraBank.Client")
    .WithReference(api).WaitFor(api)
    .WithHttpEndpoint(port: 5173, targetPort: 5174, env: "PORT")
    .PublishAsDockerFile();

builder.Build().Run();
