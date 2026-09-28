using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using microsoft_hackathon_roi_calculator.Application.Configuration;
using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Application.Services;
using microsoft_hackathon_roi_calculator.Application.UseCases;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.AddServiceDefaults();

builder.Services.AddSingleton<IROICalculatorService, ROICalculatorService>();

// Hybrid AI Reporting (Azure OpenAI with graceful offline local fallback)
builder.Services.Configure<AzureOpenAIOptions>(builder.Configuration.GetSection(AzureOpenAIOptions.SectionName));
builder.Services.AddSingleton<LocalTemplateReportService>();
builder.Services.AddSingleton<IAssistantReportService, AzureOpenAIReportService>();

builder.Build().Run();