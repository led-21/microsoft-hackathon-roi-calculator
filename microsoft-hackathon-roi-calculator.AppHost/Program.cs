var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

var chat = builder.AddOllama("chat")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var phi4 = chat.AddModel("phi4", "phi4");

var fuctions = builder.AddAzureFunctionsProject<Projects.microsoft_hackathon_roi_calculator_Functions>("functions")
    .WithHostStorage(storage)
    .WaitFor(storage);

var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("roidb");

var migrations = builder.AddProject<Projects.microsoft_hackathon_roi_calculator_MigrationService>("migrations")
    .WithReference(sql)
    .WaitFor(sql);

var apiService = builder.AddProject<Projects.microsoft_hackathon_roi_calculator_Api>("apiservice")
    .WithReference(cache)
    .WithReference(sql)
    .WithReference(phi4)
    .WaitFor(phi4)
    .WaitForCompletion(migrations);

builder.AddProject<Projects.microsoft_hackathon_roi_calculator_Web>("frontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(fuctions)
    .WaitFor(fuctions);

builder.Build().Run();