using microsoft_hackathon_roi_calculator.Core.Models;
using microsoft_hackathon_roi_calculator.Core.Interfaces;

namespace microsoft_hackathon_roi_calculator.ApiService.Endpoints;
static public class ROIEndpoint
{
    static public void AddROIEndpoint(this WebApplication app)
    {
        app.MapPost("/api/roi/analyze", (ROICalculationMetrics metrics, IROICalculatorService roiService) =>
        {
            var result = new
            {
                ROI = roiService.CalculateROI(metrics),
                ProcessComplianceRate = roiService.CalculateProcessComplianceRate(metrics),
                TrainingCompletionRate = roiService.CalculateTrainingCompletionRate(metrics),
                EmployeeAdoptionRate = roiService.CalculateEmployeeAdoptionRate(metrics),
                AverageScore = roiService.CalculateAverageScore(metrics),
                PositiveResponseRate = roiService.CalculatePositiveResponseRate(metrics),
                TrainingScoreImprovement = roiService.CalculateTrainingScoreImprovement(metrics),
                ImplementationEfficiency = roiService.CalculateImplementationEfficiency(metrics)
            };
            return Results.Ok(result);
        })
        .WithName("GetROI");
    }
}