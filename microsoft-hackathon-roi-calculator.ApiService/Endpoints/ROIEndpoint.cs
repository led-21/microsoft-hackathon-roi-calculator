
using Microsoft.EntityFrameworkCore;
using microsoft_hackathon_roi_calculator.ApiService.Infrastructure.Data;
using microsoft_hackathon_roi_calculator.Core.Models;

namespace microsoft_hackathon_roi_calculator.ApiService.Endpoints;
static public class ROIEndpoint
{
    static public void AddROIEndpoint(this WebApplication app)
    {

        app.MapGet("/api/roi", async (CalculatorDbContext dbContext) =>
        {
            var roi = await dbContext.ProjectROIs.ToListAsync();
            return Results.Ok(roi);
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
            if (existingROI == null) return Results.NotFound();

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
            if (existingROI == null) return Results.NotFound();

            dbContext.ProjectROIs.Remove(existingROI);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}