using microsoft_hackathon_roi_calculator.Domain.Models;

namespace microsoft_hackathon_roi_calculator.Domain.Financial;

/// <summary>
/// Immutable, strongly-typed result of deterministic financial ROI modeling.
/// </summary>
public record ROICalculationResult
{
    public double TotalInvestment { get; init; }
    public double TotalBenefits { get; init; }
    public double NetBenefit => TotalBenefits - TotalInvestment;
    public double RoiPercentage { get; init; }
    public double BenefitCostRatio => TotalInvestment > 0 ? TotalBenefits / TotalInvestment : 0.0;
    public bool IsViable => RoiPercentage > 0;

    // Component details
    public double AverageEmployeeCostPerMonth { get; init; }
    public double ProductivityGainValue { get; init; }
    public double AdjustedProductivityGainValue { get; init; }
    public double RiskReductionValue { get; init; }
    public double AdjustedRiskReduction { get; init; }
    public double SuccessBenefitValue { get; init; }
    public double AdjustedSuccessBenefit { get; init; }

    /// <summary>
    /// Theoretical maximum project failure rate before ROI turns negative (Break-even).
    /// Returns a value between 0.0 and 1.0, or double.NaN if not applicable.
    /// </summary>
    public double BreakEvenFailureRate { get; init; }

    public IReadOnlyList<string> ActionableRecommendations { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Converts to the legacy ROICalculationResults model for API compatibility.
    /// </summary>
    public ROICalculationResults ToLegacyModel()
    {
        return new ROICalculationResults
        {
            TotalInvestment = TotalInvestment,
            TotalBenefits = TotalBenefits,
            RoiPercentage = RoiPercentage,
            ProductivityGainValue = ProductivityGainValue,
            AdjustedProductivityGainValue = AdjustedProductivityGainValue,
            RiskReductionValue = RiskReductionValue,
            AdjustedRiskReduction = AdjustedRiskReduction,
            SuccessBenefitValue = SuccessBenefitValue,
            AdjustedSuccessBenefit = AdjustedSuccessBenefit,
            ActionableRecommendations = ActionableRecommendations.ToList()
        };
    }
}
