using microsoft_hackathon_roi_calculator.Core.Models;

namespace microsoft_hackathon_roi_calculator.Core.Interfaces
{
    public interface IROICalculatorService
    {
        decimal CalculateROI(ROICalculationMetrics metrics);
        decimal CalculateProcessComplianceRate(ROICalculationMetrics metrics);
        decimal CalculateTrainingCompletionRate(ROICalculationMetrics metrics);
        decimal CalculateEmployeeAdoptionRate(ROICalculationMetrics metrics);
        decimal CalculateAverageScore(ROICalculationMetrics metrics);
        decimal CalculatePositiveResponseRate(ROICalculationMetrics metrics);
        int CalculateTrainingScoreImprovement(ROICalculationMetrics metrics);
        decimal CalculateImplementationEfficiency(ROICalculationMetrics metrics);
    }
}
