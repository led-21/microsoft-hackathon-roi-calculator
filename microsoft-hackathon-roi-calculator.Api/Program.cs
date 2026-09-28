using Microsoft.EntityFrameworkCore;
using microsoft_hackathon_roi_calculator.Persistence.Data;
using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Application.UseCases;
using microsoft_hackathon_roi_calculator.Application.Services;
using microsoft_hackathon_roi_calculator.Application.Configuration;
using microsoft_hackathon_roi_calculator.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

// Hybrid / Offline Persistence configuration
var sqlConnection = builder.Configuration.GetConnectionString("roidb");
if (!string.IsNullOrWhiteSpace(sqlConnection))
{
    builder.AddSqlServerDbContext<CalculatorDbContext>("roidb");
}
else
{
    // Local SQLite fallback when Azure SQL / SQL Server container is unavailable
    builder.Services.AddDbContext<CalculatorDbContext>(options =>
    {
        options.UseSqlite("Data Source=roidb.db");
    });
}

// Hybrid / Offline Cache configuration
var redisConnection = builder.Configuration.GetConnectionString("cache");
if (!string.IsNullOrWhiteSpace(redisConnection))
{
    builder.AddRedisDistributedCache("cache");
}
else
{
    // In-memory cache fallback when Redis container is unavailable
    builder.Services.AddDistributedMemoryCache();
}

// Core calculation engine
builder.Services.AddSingleton<IROICalculatorService, ROICalculatorService>();

// Hybrid AI Report Services (Azure OpenAI with automatic local fallback)
builder.Services.Configure<AzureOpenAIOptions>(builder.Configuration.GetSection(AzureOpenAIOptions.SectionName));
builder.Services.AddSingleton<LocalTemplateReportService>();
builder.Services.AddSingleton<IAssistantReportService, AzureOpenAIReportService>();

// Optional Ollama Client (supported when running locally via Aspire)
if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("phi4")))
{
    builder.AddOllamaApiClient("phi4");
}

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "InovaROI Platform API", 
        Version = "v1",
        Description = "ROI Calculation, Machine Learning Risk Estimation, and Generative Reporting API."
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostAndClients", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        {
            if (Uri.TryCreate(origin, UriKind.Absolute, out var uri))
            {
                return uri.Host == "localhost" || uri.Host == "127.0.0.1";
            }
            return false;
        })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowLocalhostAndClients");
app.UseExceptionHandler();

// Ensure database schema is created for local/standalone execution
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CalculatorDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "InovaROI API v1");
        c.RoutePrefix = "swagger";
    });
}

app.AddROIEndpoint();
app.MapDefaultEndpoints();

app.Run();
