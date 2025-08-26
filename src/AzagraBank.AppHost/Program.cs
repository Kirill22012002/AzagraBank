var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("PostgreSQL")
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var azagraBankDb = postgres.AddDatabase("azagra-bank-db");

var azagraBankCache = builder.AddRedis("Redis");

var kafka = builder.AddKafka("Kafka")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithKafkaUI();

var commandsProcessor = builder.AddProject<Projects.AzagraBank_CommandProcessor>("CommandsProcessor")
    .WithReference(kafka).WaitFor(kafka);

var api = builder.AddProject<Projects.AzagraBank_ApiService>("Api")
    .WithReference(commandsProcessor).WaitFor(commandsProcessor)
    .WithReference(azagraBankCache).WaitFor(azagraBankCache)
    .WithExternalHttpEndpoints();

var client = builder.AddNpmApp("Client", "../AzagraBank.Client")
    .WithReference(api).WaitFor(api)
    .WithHttpEndpoint(port: 5173, targetPort: 5174, env: "PORT")
    .PublishAsDockerFile();


builder.Build().Run();
