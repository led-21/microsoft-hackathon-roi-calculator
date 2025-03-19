using microsoft_hackathon_roi_calculator.Persistence.Data;
using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Application.UseCases;
using microsoft_hackathon_roi_calculator.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddRedisDistributedCache("cache");
builder.AddSqlServerDbContext<CalculatorDbContext>("roidb");

builder.Services.AddSingleton<IROICalculatorService, ROICalculatorService>();

builder.AddOllamaApiClient("phi4");

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ROI Calculator API", Version = "v1" });
});

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyLocalhostPort",
       builder =>
       {
           builder.SetIsOriginAllowed(origin =>
           {
               // Permite qualquer porta do localhost (HTTP ou HTTPS)
               return new Uri(origin).Host == "localhost";
           })
           .AllowAnyHeader()
           .AllowAnyMethod();
       });

});

var app = builder.Build();

app.UseCors("AllowAnyLocalhostPort");

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ROI Calculator API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.AddROIEndpoint();

app.MapDefaultEndpoints();

app.Run();