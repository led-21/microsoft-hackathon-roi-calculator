using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using microsoft_hackathon_roi_calculator.Domain.Financial;
using microsoft_hackathon_roi_calculator.Domain.Models;
using microsoft_hackathon_roi_calculator.Persistence.Data;

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
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(CalculatorDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.ProjectROIs.AnyAsync(cancellationToken))
        {
            return;
        }

        var random = new Random(42);
        var projects = new List<ProjectROI>();

        List<string> names = new()
        {
            "Aurora", "Nexus", "Zenith", "Quantum", "Vanguarda",
            "Epica", "Horizonte", "Prisma", "Fenix", "Nova Era",
            "Inovacao", "Pioneiro", "Estrela", "Orion", "Galaxia",
            "Infinito", "Explorador", "Eclipse", "Miragem", "Cosmos"
        };

        for (int i = 1; i <= 50; i++)
        {
            int employeeNumber = random.Next(5, 120);
            double projectBudget = random.Next(20_000, 2_000_000);
            int projectDuration = random.Next(3, 24);

            double estimatedRoi = CalculateConsistentROI(projectBudget, employeeNumber, projectDuration);

            projects.Add(new ProjectROI
            {
                ProjectName = $"Projeto {names[random.Next(0, names.Count)]} {names[random.Next(0, names.Count)]} #{i}",
                Description = "Iniciativa de transformacao e adocao de novas tecnologias.",
                ProjectBudget = projectBudget,
                NumberOfEmployees = employeeNumber,
                StartDate = DateTime.UtcNow.AddMonths(-random.Next(1, 12)),
                ProjectDurationMonths = projectDuration,
                ROI = estimatedRoi,
                TotalHoursWorkedWeekly = employeeNumber * 40,
                CompletedTraining = random.Next(1, employeeNumber),
                EmployeesUsingNewTool = random.Next(1, employeeNumber),
                TotalChangeImplementationTime = projectDuration * 4,
                TotalPlannedImplementationTime = projectDuration * 4,
                TotalProcesses = random.Next(5, 30),
                CompliantProcesses = random.Next(3, 25),
                ProjectEvaluationTotalResponses = employeeNumber,
                ProjectEvaluationPositiveResponses = (int)(employeeNumber * (0.6 + (random.NextDouble() * 0.35))),
                ProjectEvaluationSumOfAllScores = employeeNumber * random.Next(7, 10)
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

    private static double CalculateConsistentROI(double projectBudget, int numberOfEmployees, int projectDurationMonths)
    {
        try
        {
            var inputs = new FinancialInputs(projectBudget, numberOfEmployees, projectDurationMonths);
            var model = new FinancialModel(inputs);
            var result = ROICalculator.Calculate(model);
            return Math.Round(result.RoiPercentage / 100.0, 2);
        }
        catch
        {
            return 0.15;
        }
    }
}
