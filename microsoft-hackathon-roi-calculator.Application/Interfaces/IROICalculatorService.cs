using microsoft_hackathon_roi_calculator.Domain.Financial;
using microsoft_hackathon_roi_calculator.Domain.Models;

namespace microsoft_hackathon_roi_calculator.Application.Interfaces;

public interface IROICalculatorService
{
    double EstimateFailureRate(ROIInputParameters input);
    ROICalculationResults CalculateROI(ROIInputParameters input);
    ROICalculationResult Calculate(FinancialModel model);
    string GenerateReport(ROICalculationResults result, ROIInputParameters input);
}