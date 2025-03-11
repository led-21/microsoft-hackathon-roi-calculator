var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("TesteDB");

var apiService = builder.AddProject<Projects.microsoft_hackathon_roi_calculator_ApiService>("apiservice")
    .WithReference(sql)
    .WaitFor(sql);

var migrations = builder.AddProject<Projects.microsoft_hackathon_roi_calculator_MigrationService>("migrations")
        .WithReference(sql)
        .WaitFor(sql);

builder.AddProject<Projects.microsoft_hackathon_roi_calculator_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
