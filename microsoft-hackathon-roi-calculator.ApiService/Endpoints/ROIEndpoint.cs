using microsoft_hackathon_roi_calculator.Core.Interfaces;
using microsoft_hackathon_roi_calculator.Core.Models.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace microsoft_hackathon_roi_calculator.ApiService.Endpoints;
static public class ROIEndpoint
{
    static public void AddROIEndpoint(this WebApplication app)
    {
        // Endpoints da API
        app.MapPost("/api/roi", ([FromBody] CostBenefitMetrics metrics, IROICalculatorService service) =>
        {
            return Results.Ok(service.CalculateROI(metrics));
        })
        .WithName("CalculateROI")
        .WithOpenApi();

        app.MapPost("/api/process-compliance", ([FromBody] ProcessMetrics metrics, IROICalculatorService service) =>
        {
            try
            {
                return Results.Ok(service.CalculateProcessComplianceRate(metrics));
            }
            catch (ArgumentNullException ex)
            {
                return Results.BadRequest(new { Error = "Os dados fornecidos não podem ser nulos.", Details = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
        })
        .WithName("CalculateProcessComplianceRate")
        .WithOpenApi();

        app.MapPost("/api/training-completion", ([FromBody] TrainingMetrics metrics, IROICalculatorService service) =>
        {
            try
            {
                return Results.Ok(service.CalculateTrainingCompletionRate(metrics));
            }
            catch (ArgumentNullException ex)
            {
                return Results.BadRequest(new { Error = "Os dados fornecidos não podem ser nulos.", Details = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
        })
        .WithName("CalculateTrainingCompletionRate")
        .WithOpenApi();

        app.MapPost("/api/employee-adoption", ([FromBody] EmployeeMetrics metrics, IROICalculatorService service) =>
        {
            try
            {
                return Results.Ok(service.CalculateEmployeeAdoptionRate(metrics));
            }
            catch (ArgumentNullException ex)
            {
                return Results.BadRequest(new { Error = "Os dados fornecidos não podem ser nulos.", Details = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
        })
        .WithName("CalculateEmployeeAdoptionRate")
        .WithOpenApi();

        app.MapPost("/api/average-score", ([FromBody] ResponseMetrics metrics, IROICalculatorService service) =>
        {
            try
            {
                return Results.Ok(service.CalculateAverageScore(metrics));
            }
            catch (ArgumentNullException ex)
            {
                return Results.BadRequest(new { Error = "Os dados fornecidos não podem ser nulos.", Details = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
        })
        .WithName("CalculateAverageScore")
        .WithOpenApi();

        app.MapPost("/api/positive-response", ([FromBody] ResponseMetrics metrics, IROICalculatorService service) =>
        {
            try
            {
                return Results.Ok(service.CalculatePositiveResponseRate(metrics));
            }
            catch (ArgumentNullException ex)
            {
                return Results.BadRequest(new { Error = "Os dados fornecidos não podem ser nulos.", Details = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
        })
        .WithName("CalculatePositiveResponseRate")
        .WithOpenApi();

        app.MapPost("/api/training-score-improvement", ([FromBody] TrainingMetrics metrics, IROICalculatorService service) =>
        {
            try
            {
                return Results.Ok(service.CalculateTrainingScoreImprovement(metrics));
            }
            catch (ArgumentNullException ex)
            {
                return Results.BadRequest(new { Error = "Os dados fornecidos não podem ser nulos.", Details = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
        })
        .WithName("CalculateTrainingScoreImprovement")
        .WithOpenApi();

        app.MapPost("/api/implementation-efficiency", ([FromBody] ImplementationMetrics metrics, IROICalculatorService service) =>
        {
            try
            {
                return Results.Ok(service.CalculateImplementationEfficiency(metrics));
            }
            catch (ArgumentNullException ex)
            {
                return Results.BadRequest(new { Error = "Os dados fornecidos não podem ser nulos.", Details = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { Error = "Dados inválidos fornecidos.", Details = ex.Message });
            }
        })
        .WithName("CalculateImplementationEfficiency")
        .WithOpenApi();
    }
}