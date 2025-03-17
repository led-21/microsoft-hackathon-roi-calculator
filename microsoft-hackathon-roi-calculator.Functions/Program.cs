using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Application.UseCases;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.AddServiceDefaults();

builder.Services.AddSingleton<IROICalculatorService, ROICalculatorService>();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAnyLocalhostPort",
//       builder =>
//       {
//           builder.SetIsOriginAllowed(origin =>
//           {
//               // Permite qualquer porta do localhost (HTTP ou HTTPS)
//               return new Uri(origin).Host == "localhost";
//           })
//           .AllowAnyHeader()
//           .AllowAnyMethod();
//       });

//});

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();