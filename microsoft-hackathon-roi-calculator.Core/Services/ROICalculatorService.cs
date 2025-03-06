using microsoft_hackathon_roi_calculator.Core.Interfaces;
using microsoft_hackathon_roi_calculator.Core.Models;

namespace microsoft_hackathon_roi_calculator.Core.Services
{
    public class ROICalculatorService : IROICalculatorService
    {
        private const decimal PERCENTAGE_MULTIPLIER = 100m;
        private const decimal DEFAULT_RESULT = 0m;

        private decimal CalculatePercentage(decimal numerator, decimal denominator)
        {
            return denominator == 0 ? DEFAULT_RESULT : (numerator / denominator) * PERCENTAGE_MULTIPLIER;
        }

        private void ValidateMetrics(ROICalculationMetrics metrics)
        {
            ArgumentNullException.ThrowIfNull(metrics, nameof(metrics));
        }

        public decimal CalculateROI(ROICalculationMetrics metrics)
        {
            ValidateMetrics(metrics);
            return CalculatePercentage(metrics.NetBenefit - metrics.CostOfInvestment, metrics.CostOfInvestment);
        }

        public decimal CalculateProcessComplianceRate(ROICalculationMetrics metrics)
        {
            ValidateMetrics(metrics);
            return CalculatePercentage(metrics.CompliantProcesses, metrics.TotalProcesses);
        }

        public decimal CalculateTrainingCompletionRate(ROICalculationMetrics metrics)
        {
            ValidateMetrics(metrics);
            return CalculatePercentage(metrics.CompletedTraining, metrics.EnrolledTraining);
        }

        public decimal CalculateEmployeeAdoptionRate(ROICalculationMetrics metrics)
        {
            ValidateMetrics(metrics);
            return CalculatePercentage(metrics.EmployeesUsingNewTool, metrics.TotalEmployees);
        }

        public decimal CalculateAverageScore(ROICalculationMetrics metrics)
        {
            ValidateMetrics(metrics);
            return metrics.TotalResponses == 0 ? DEFAULT_RESULT : (decimal)metrics.SumOfAllScores / metrics.TotalResponses;
        }

        public decimal CalculatePositiveResponseRate(ROICalculationMetrics metrics)
        {
            ValidateMetrics(metrics);
            return CalculatePercentage(metrics.PositiveResponses, metrics.TotalResponses);
        }

        public int CalculateTrainingScoreImprovement(ROICalculationMetrics metrics)
        {
            ValidateMetrics(metrics);
            return metrics.PostTrainingScore - metrics.PreTrainingScore;
        }

        public decimal CalculateImplementationEfficiency(ROICalculationMetrics metrics)
        {
            ValidateMetrics(metrics);
            return CalculatePercentage(metrics.TotalChangeImplementationTime, metrics.TotalPlannedImplementationTime);
        }
    }
}
