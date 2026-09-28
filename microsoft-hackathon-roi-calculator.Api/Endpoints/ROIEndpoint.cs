using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Globalization;
using microsoft_hackathon_roi_calculator.Domain.Models;
using microsoft_hackathon_roi_calculator.Persistence.Data;
using microsoft_hackathon_roi_calculator.Application.Interfaces;

namespace microsoft_hackathon_roi_calculator.Api.Endpoints;

public static class ROIEndpoint
{
    private const string CacheKey = "roi_projects";

    public static void AddROIEndpoint(this WebApplication app)
    {
        // 1. AI Narrative Report Generation (Azure OpenAI with local offline fallback)
        app.MapPost("/api/roi/ai", async (HttpRequest request, IAssistantReportService assistantService, ILogger<WebApplication> logger, CancellationToken ct) =>
        {
            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            string report = await reader.ReadToEndAsync(ct);

            if (string.IsNullOrWhiteSpace(report))
            {
                return Results.BadRequest("Report content cannot be empty.");
            }

            try
            {
                var insights = await assistantService.GenerateInsightsAsync(report, ct);
                return Results.Content(insights, "text/plain; charset=utf-8");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred during AI report generation.");
                return Results.Problem("Ocorreu um erro ao processar o relatório de IA.");
            }
        }).WithRequestTimeout(TimeSpan.FromMinutes(1));

        // 2. Deterministic ROI Calculation & Detailed Markdown Breakdown
        app.MapPost("/api/roi/calculator", (IROICalculatorService calculator, ROIInputParameters input) =>
        {
            try
            {
                var result = calculator.CalculateROI(input);
                var report = calculator.GenerateReport(result, input);
                return Results.Content(report, "text/plain; charset=utf-8");
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        // 3. Machine Learning (ML.NET) Project Failure Rate Estimation
        app.MapPost("/api/roi/estimate/failure-rate", (IROICalculatorService calculator, ROIInputParameters input) =>
        {
            try
            {
                var estimate = calculator.EstimateFailureRate(input);
                if (double.IsNaN(estimate) || double.IsInfinity(estimate))
                    return Results.BadRequest("Não foi possível estimar a taxa de falha.");

                return Results.Ok((estimate * 100).ToString("0.00", CultureInfo.InvariantCulture) + "%");
            }
            catch
            {
                return Results.BadRequest("Erro ao calcular taxa de falha.");
            }
        });

        // 4. Project Data Export (CSV)
        app.MapGet("/api/roi/csv", async (CalculatorDbContext dbContext, ILogger<WebApplication> logger, CancellationToken ct) =>
        {
            try
            {
                var roiList = await dbContext.ProjectROIs.AsNoTracking().ToListAsync(ct);
                var csv = new StringBuilder();
                csv.AppendLine("ProjectName,ProjectBudget,NumberOfEmployees,ProjectDurationMonths,ROI");

                foreach (var roi in roiList)
                {
                    csv.AppendLine($"{roi.ProjectName},{roi.ProjectBudget.ToString(CultureInfo.InvariantCulture)},{roi.NumberOfEmployees},{roi.ProjectDurationMonths},{roi.ROI.ToString(CultureInfo.InvariantCulture)}");
                }

                var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
                return Results.File(csvBytes, "text/csv", "roi_projects.csv");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to generate CSV export.");
                return Results.Problem("Erro ao gerar o arquivo CSV.");
            }
        });

        // 5. Query Historical Projects with Distributed/In-Memory Cache
        app.MapGet("/api/roi", async (CalculatorDbContext dbContext, IDistributedCache cache, ILogger<WebApplication> logger, CancellationToken ct) =>
        {
            try
            {
                var cachedJson = await cache.GetStringAsync(CacheKey, ct);
                if (!string.IsNullOrEmpty(cachedJson))
                {
                    var cachedProjects = JsonSerializer.Deserialize<List<ProjectROI>>(cachedJson);
                    if (cachedProjects != null)
                        return Results.Ok(cachedProjects);
                }

                var projects = await dbContext.ProjectROIs.AsNoTracking().ToListAsync(ct);

                await cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(projects), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                }, ct);

                return Results.Ok(projects);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cache read failed, falling back to direct database query.");
                var projects = await dbContext.ProjectROIs.AsNoTracking().ToListAsync(ct);
                return Results.Ok(projects);
            }
        });

        // 6. Project CRUD (Create, Update, Delete) with Cache Invalidation
        app.MapPost("/api/roi", async (CalculatorDbContext dbContext, IDistributedCache cache, ProjectROI newROI, ILogger<WebApplication> logger, CancellationToken ct) =>
        {
            try
            {
                dbContext.ProjectROIs.Add(newROI);
                await dbContext.SaveChangesAsync(ct);
                await cache.RemoveAsync(CacheKey, ct);

                return Results.Created($"/api/roi/{newROI.Id}", newROI);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create new ProjectROI.");
                return Results.Problem("Erro ao criar o projeto.");
            }
        });

        app.MapPut("/api/roi/{id}", async (CalculatorDbContext dbContext, IDistributedCache cache, int id, ProjectROI updatedROI, ILogger<WebApplication> logger, CancellationToken ct) =>
        {
            try
            {
                var existingROI = await dbContext.ProjectROIs.FindAsync([id], cancellationToken: ct);
                if (existingROI == null)
                    return Results.NotFound();

                existingROI.ProjectName = updatedROI.ProjectName;
                existingROI.ProjectBudget = updatedROI.ProjectBudget;
                existingROI.NumberOfEmployees = updatedROI.NumberOfEmployees;
                existingROI.ProjectDurationMonths = updatedROI.ProjectDurationMonths;
                existingROI.ROI = updatedROI.ROI;
                existingROI.Description = updatedROI.Description;

                await dbContext.SaveChangesAsync(ct);
                await cache.RemoveAsync(CacheKey, ct);

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update ProjectROI with ID {Id}.", id);
                return Results.Problem("Erro ao atualizar o projeto.");
            }
        });

        app.MapDelete("/api/roi/{id}", async (CalculatorDbContext dbContext, IDistributedCache cache, int id, ILogger<WebApplication> logger, CancellationToken ct) =>
        {
            try
            {
                var existingROI = await dbContext.ProjectROIs.FindAsync([id], cancellationToken: ct);
                if (existingROI == null)
                    return Results.NotFound();

                dbContext.ProjectROIs.Remove(existingROI);
                await dbContext.SaveChangesAsync(ct);
                await cache.RemoveAsync(CacheKey, ct);

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete ProjectROI with ID {Id}.", id);
                return Results.Problem("Erro ao excluir o projeto.");
            }
        });
    }
}
