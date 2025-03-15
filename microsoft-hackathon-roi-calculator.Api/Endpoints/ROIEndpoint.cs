using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Globalization;
using microsoft_hackathon_roi_calculator.Domain.Models;
using microsoft_hackathon_roi_calculator.Persistence.Data;

namespace microsoft_hackathon_roi_calculator.Api.Endpoints;
static public class ROIEndpoint
{
    static public void AddROIEndpoint(this WebApplication app)
    {
        app.MapGet("/api/roi/csv", async (CalculatorDbContext dbContext) =>
        {
            var roiList = await dbContext.ProjectROIs.ToListAsync();
            var csv = new StringBuilder();
            csv.AppendLine("ProjectName,ProjectBudget,NumberOfEmployees,ProjectDurationMonths,ROI");

            foreach (var roi in roiList)
            {
                csv.AppendLine($"{roi.ProjectName},{roi.ProjectBudget.ToString(CultureInfo.InvariantCulture)},{roi.NumberOfEmployees},{roi.ProjectDurationMonths},{roi.ROI.ToString(CultureInfo.InvariantCulture)}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
            return Results.File(csvBytes, "text/csv", "roi.csv");
        });

        app.MapGet("/api/roi", async (CalculatorDbContext dbContext, IDistributedCache cache) =>
        {
            var cachedRoi = cache.Get("roi");

            if (cachedRoi == null)
            {
                var roi = await dbContext.ProjectROIs.ToListAsync();

                await cache.SetAsync("roi", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(roi)), new()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(10)
                });

                return Results.Ok(roi);
            }

            return Results.Ok(JsonSerializer.Deserialize<IEnumerable<ProjectROI>>(cachedRoi));
        });

        app.MapPost("/api/roi", async (CalculatorDbContext dbContext, ProjectROI newROI) =>
        {
            dbContext.ProjectROIs.Add(newROI);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/api/roi/{newROI.Id}", newROI);
        });

        app.MapPut("/api/roi/{id}", async (CalculatorDbContext dbContext, int id, ProjectROI updatedROI) =>
        {
            var existingROI = await dbContext.ProjectROIs.FindAsync(id);

            if (existingROI == null)
                return Results.NotFound();

            existingROI.ProjectName = updatedROI.ProjectName;
            existingROI.ProjectBudget = updatedROI.ProjectBudget;
            existingROI.NumberOfEmployees = updatedROI.NumberOfEmployees;
            existingROI.ProjectDurationMonths = updatedROI.ProjectDurationMonths;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/api/roi/{id}", async (CalculatorDbContext dbContext, int id) =>
        {
            var existingROI = await dbContext.ProjectROIs.FindAsync(id);

            if (existingROI == null)
                return Results.NotFound();

            dbContext.ProjectROIs.Remove(existingROI);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}