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

        for (int i = 1; i <= 50; i++)
        {
            int employeeNumber = random.Next(1, 100);
            double projectBudget = random.Next(1000, 10000) * employeeNumber;
            int projectDurantion = random.Next(1, 36);

            projects.Add(new ProjectROI
            {
                ProjectName = $"Projecto {names[random.Next(0, names.Count - 1)]} {names[random.Next(0, names.Count - 1)]} {random.Next(0, 20)}",
                ProjectBudget = projectBudget, // Número de funcionários (1 a 100) x o custo médio de cada funcionario (R$1,000 a R$10,000)
                NumberOfEmployees = employeeNumber,   // Empregados impactados de 1 a 100
                ProjectDurationMonths = projectDurantion, //  Duração do projeto 1 a 36 meses
                ROI = Math.Round(-1.6 * Math.Pow(projectBudget / 1000000, 2) + 1.8 * (employeeNumber / 100) * (projectBudget / 1000000) - 0.5 * (projectDurantion / 36) + 0.3, 2) // Returno do investimento entre -0.4 a 1.0
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
}