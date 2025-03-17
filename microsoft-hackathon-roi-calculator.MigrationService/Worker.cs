using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using microsoft_hackathon_roi_calculator.Domain.Models;

namespace microsoft_hackathon_roi_calculator.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CalculatorDbContext>();

            await RunMigrationAsync(dbContext, stoppingToken);
            await SeedDataAsync(dbContext, stoppingToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(CalculatorDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(CalculatorDbContext dbContext, CancellationToken cancellationToken)
    {
        var random = new Random();
        var projects = new List<ProjectROI>();

        List<string> names = new List<string>
        {
            "Aurora", "Nexus", "Zenith", "Quantum", "Vanguarda",
            "Épica", "Horizonte", "Prisma", "Fênix", "Nova Era",
            "Inovação", "Pioneiro", "Estrela", "Orion", "Galáxia",
            "Infinito","Explorador", "Eclipse", "Miragem", "Cosmos"
        };

        for (int i = 1; i <= 100; i++)
        {
            int employeeNumber = random.Next(1, 100);
            double projectBudget = random.Next(1_000, 10_000_000);
            int projectDuration = random.Next(1, 36);

            projects.Add(new ProjectROI
            {
                ProjectName = $"Projeto {names[random.Next(0, names.Count - 1)]} {names[random.Next(0, names.Count - 1)]} {random.Next(0, 20)}",
                ProjectBudget = projectBudget,
                NumberOfEmployees = employeeNumber,
                ProjectDurationMonths = projectDuration,
                ROI = EstimateROI(projectBudget, employeeNumber, projectDuration)
            });
        }

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            await dbContext.ProjectROIs.AddRangeAsync(projects, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static double EstimateROI(double projectBudget, int numberOfEmployees, int projectDurationMonths)
    {
        // Quadratic estimation formula for ROI
        // Coefficients a, b, c, and d are assumed for estimation purposes
        const double a = 0.00000001;
        const double b = 0.0001;
        const double c = 0.0005;
        const double d = -0.5;

        double roi = a * projectBudget + b * numberOfEmployees + c * projectDurationMonths + d;

        // Clamp ROI to be within the range of -0.5 to 1.5
        return new Random().NextDouble() * 2 - 0.5;
    }
}