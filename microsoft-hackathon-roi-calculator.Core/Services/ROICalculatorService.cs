using microsoft_hackathon_roi_calculator.Core.Interfaces;
using microsoft_hackathon_roi_calculator.Core.Models;

namespace microsoft_hackathon_roi_calculator.Core.Services
{
    public class ROICalculatorService : IROICalculatorService
    {
        public decimal CalculateROI(ROICalculationMetrics metrics)
        {
            if (metrics.CostOfInvestment == 0) return 0;
            return (metrics.NetBenefit - metrics.CostOfInvestment) / metrics.CostOfInvestment * 100;
        }

        public decimal CalculateProcessComplianceRate(ROICalculationMetrics metrics)
        {
            if (metrics.TotalProcesses == 0) return 0;
            return (decimal)metrics.CompliantProcesses / metrics.TotalProcesses * 100;
        }

        public decimal CalculateTrainingCompletionRate(ROICalculationMetrics metrics)
        {
            if (metrics.EnrolledTraining == 0) return 0;
            return (decimal)metrics.CompletedTraining / metrics.EnrolledTraining * 100;
        }

        public decimal CalculateEmployeeAdoptionRate(ROICalculationMetrics metrics)
        {
            if (metrics.TotalEmployees == 0) return 0;
            return (decimal)metrics.EmployeesUsingNewTool / metrics.TotalEmployees * 100;
        }

        public decimal CalculateAverageScore(ROICalculationMetrics metrics)
        {
            if (metrics.TotalResponses == 0) return 0;
            return (decimal)metrics.SumOfAllScores / metrics.TotalResponses;
        }

        public decimal CalculatePositiveResponseRate(ROICalculationMetrics metrics)
        {
            if (metrics.TotalResponses == 0) return 0;
            return (decimal)metrics.PositiveResponses / metrics.TotalResponses * 100;
        }

        public int CalculateTrainingScoreImprovement(ROICalculationMetrics metrics)
        {
            return metrics.PostTrainingScore - metrics.PreTrainingScore;
        }

        public decimal CalculateImplementationEfficiency(ROICalculationMetrics metrics)
        {
            if (metrics.TotalPlannedImplementationTime == 0) return 0;
            return (decimal)metrics.TotalChangeImplementationTime / metrics.TotalPlannedImplementationTime * 100;
        }
    }
}
