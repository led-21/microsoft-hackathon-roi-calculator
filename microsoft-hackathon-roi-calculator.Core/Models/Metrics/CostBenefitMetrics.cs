namespace microsoft_hackathon_roi_calculator.Core.Models.Metrics;

/// <summary>
/// Métricas relacionadas a custos e benefícios
/// </summary>
public class CostBenefitMetrics
{
    public decimal NetBenefit { get; set; } // *Net benefit = Total benefits − total costs
    public decimal CostOfInvestment { get; set; }

    // ROI (%) = (Net benefit* ÷ cost of investment) × 100
    public decimal GetReturnOnInvestment() => NetBenefit / CostOfInvestment * 100;
}