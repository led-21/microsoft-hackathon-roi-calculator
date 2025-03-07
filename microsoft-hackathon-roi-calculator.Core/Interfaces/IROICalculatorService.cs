using microsoft_hackathon_roi_calculator.Core.Models.Metrics;

namespace microsoft_hackathon_roi_calculator.Core.Interfaces;
public interface IROICalculatorService
{
    decimal CalculateROI(CostBenefitMetrics metrics);
    decimal CalculateProcessComplianceRate(ProcessMetrics metrics);
    decimal CalculateTrainingCompletionRate(TrainingMetrics metrics);
    decimal CalculateEmployeeAdoptionRate(EmployeeMetrics metrics);
    decimal CalculateAverageScore(ResponseMetrics metrics);
    decimal CalculatePositiveResponseRate(ResponseMetrics metrics);
    int CalculateTrainingScoreImprovement(TrainingMetrics metrics);
    decimal CalculateImplementationEfficiency(ImplementationMetrics metrics);
}

