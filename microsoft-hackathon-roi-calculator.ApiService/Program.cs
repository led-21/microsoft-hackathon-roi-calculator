using microsoft_hackathon_roi_calculator.Core.Services;
using microsoft_hackathon_roi_calculator.Core.Interfaces;
using microsoft_hackathon_roi_calculator.ApiService.Endpoints;


var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ROI Calculator API", Version = "v1" });
});

builder.Services.AddScoped<IROICalculatorService ,ROICalculatorService>();

var app = builder.Build();

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

