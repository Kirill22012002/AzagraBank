var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AzagraBank_ApiService>("azagrabank-apiservice");

builder.Build().Run();
