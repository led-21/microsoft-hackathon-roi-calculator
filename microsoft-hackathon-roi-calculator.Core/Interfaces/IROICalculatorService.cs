using microsoft_hackathon_roi_calculator.Core.Models;

namespace microsoft_hackathon_roi_calculator.Core.Interfaces;
public interface IROICalculatorService
{
    public ROICalculationResults CalculateROI(ROIInputParameters input);
    public string GenerateReport(ROICalculationResults result, ROIInputParameters input);
}

