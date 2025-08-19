var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("pgsql").AddDatabase("azagra-bank-db");

var redis = builder.AddRedis("redis");

var api = builder.AddProject<Projects.AzagraBank_ApiService>("azagrabank-apiservice")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(redis)
    .WaitFor(redis)
    .WithExternalHttpEndpoints();

var client = builder.AddNpmApp("react", "../AzagraBank.Client")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
