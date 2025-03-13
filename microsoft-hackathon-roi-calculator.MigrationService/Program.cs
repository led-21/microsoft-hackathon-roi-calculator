using microsoft_hackathon_roi_calculator.MigrationService;
using Microsoft.EntityFrameworkCore;
using microsoft_hackathon_roi_calculator.Core.Models.Metrics;
using microsoft_hackathon_roi_calculator.Core.Models;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.AddSqlServerDbContext<CalculatorDbContext>("TesteDB");

var host = builder.Build();
host.Run();

public class CalculatorDbContext : DbContext
{
    public CalculatorDbContext(DbContextOptions<CalculatorDbContext> options) : base(options)
    {

    }

    public DbSet<ProjectROI> ProjectROIs { get; set; }

}
