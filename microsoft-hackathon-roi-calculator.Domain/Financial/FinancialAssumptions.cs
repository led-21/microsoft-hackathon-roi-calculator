namespace microsoft_hackathon_roi_calculator.Domain.Financial;

/// <summary>
/// Operational and strategic assumptions applied to the financial calculations.
/// </summary>
/// <param name="ExpectedProductivityGain">
/// Expected productivity factor relative to baseline (1.0). For example, 1.2 represents a 20% productivity increase.
/// </param>
/// <param name="BudgetLossRate">
/// Percentage of the total budget at risk of loss if the transformation fails (e.g. 0.3 for 30%).
/// </param>
/// <param name="ProjectedRiskReduction">
/// Target proportion of at-risk budget mitigated by proactive change management and tooling (e.g. 0.5 for 50%).
/// </param>
/// <param name="ExpectedSuccessBenefit">
/// Additional upside multiplier on the project budget generated upon successful deployment (e.g. 1.5 for 150%).
/// </param>
public record FinancialAssumptions
{
    public double ExpectedProductivityGain { get; init; } = 1.2;
    public double BudgetLossRate { get; init; } = 0.3;
    public double ProjectedRiskReduction { get; init; } = 0.0;
    public double ExpectedSuccessBenefit { get; init; } = 1.5;

    public FinancialAssumptions() { }

    public FinancialAssumptions(
        double expectedProductivityGain,
        double budgetLossRate,
        double projectedRiskReduction,
        double expectedSuccessBenefit)
    {
        if (expectedProductivityGain < 0)
            throw new ArgumentOutOfRangeException(nameof(expectedProductivityGain), "Productivity gain cannot be negative.");
        if (budgetLossRate < 0 || budgetLossRate > 1.0)
            throw new ArgumentOutOfRangeException(nameof(budgetLossRate), "Budget loss rate must be between 0.0 and 1.0.");
        if (projectedRiskReduction < 0 || projectedRiskReduction > 1.0)
            throw new ArgumentOutOfRangeException(nameof(projectedRiskReduction), "Projected risk reduction must be between 0.0 and 1.0.");
        if (expectedSuccessBenefit < 0)
            throw new ArgumentOutOfRangeException(nameof(expectedSuccessBenefit), "Expected success benefit multiplier cannot be negative.");

        ExpectedProductivityGain = expectedProductivityGain;
        BudgetLossRate = budgetLossRate;
        ProjectedRiskReduction = projectedRiskReduction;
        ExpectedSuccessBenefit = expectedSuccessBenefit;
    }
}
