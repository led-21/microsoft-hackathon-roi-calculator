using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using microsoft_hackathon_roi_calculator.MigrationService;
using microsoft_hackathon_roi_calculator.Persistence.Data;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.AddSqlServerDbContext<CalculatorDbContext>("roidb");

var host = builder.Build();
host.Run();