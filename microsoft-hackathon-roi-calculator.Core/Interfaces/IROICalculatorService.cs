using microsoft_hackathon_roi_calculator.Core.Models;

namespace microsoft_hackathon_roi_calculator.Core.Interfaces;
public interface IROICalculatorService
{
    public ROICalculatorResult CalculateROI(ProjectInput input);
    public string GenerateReport(ROICalculatorResult result, ProjectInput input);
}

