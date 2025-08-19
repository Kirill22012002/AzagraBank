var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.AzagraBank_ApiService>("azagrabank-apiservice");

var client = builder.AddNpmApp("react", "../AzagraBank.Client")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
