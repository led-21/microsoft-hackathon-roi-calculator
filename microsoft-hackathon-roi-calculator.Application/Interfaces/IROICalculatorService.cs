using microsoft_hackathon_roi_calculator.Domain.Models;

namespace microsoft_hackathon_roi_calculator.Application.Interfaces;
public interface IROICalculatorService
{
    public double EstimateFailureRate(ROIInputParameters input);
    public ROICalculationResults CalculateROI(ROIInputParameters input);
    public string GenerateReport(ROICalculationResults result, ROIInputParameters input);
}