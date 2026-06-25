var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireQwen_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithEnvironment("ModelPath", @"C:\Users\iAdos\.lmstudio\models\lmstudio-community\Qwen3.6-35B-A3B-GGUF\Qwen3.6-35B-A3B-Q4_K_M.gguf");

builder.AddProject<Projects.AspireQwen_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
