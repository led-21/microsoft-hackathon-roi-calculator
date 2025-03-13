using System.ComponentModel.DataAnnotations;

namespace microsoft_hackathon_roi_calculator.Core.Models.Metrics;

/// <summary>
/// Métricas relacionadas a custos e benefícios
/// </summary>
public class CostBenefitMetrics
{
    public double NetBenefit { get; set; } // *Net benefit = Total benefits − total costs
    public double CostOfInvestment { get; set; }

    // ROI (%) = (Net benefit* ÷ cost of investment) × 100
    public double ReturnOnInvestment { get => NetBenefit / CostOfInvestment * 100; }
}