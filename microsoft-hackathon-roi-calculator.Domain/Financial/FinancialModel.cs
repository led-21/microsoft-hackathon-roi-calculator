using microsoft_hackathon_roi_calculator.Domain.Models;

namespace microsoft_hackathon_roi_calculator.Domain.Financial;

/// <summary>
/// Immutable aggregate combining inputs, assumptions, and risk parameters.
/// Represents the complete financial context before calculations are executed.
/// </summary>
public record FinancialModel
{
    public FinancialInputs Inputs { get; init; }
    public FinancialAssumptions Assumptions { get; init; }
    public RiskParameters Risk { get; init; }

    public FinancialModel(
        FinancialInputs inputs,
        FinancialAssumptions? assumptions = null,
        RiskParameters? risk = null)
    {
        Inputs = inputs ?? throw new ArgumentNullException(nameof(inputs));
        Assumptions = assumptions ?? new FinancialAssumptions();
        Risk = risk ?? new RiskParameters();
    }

    /// <summary>
    /// Factory method creating a FinancialModel directly from legacy ROIInputParameters.
    /// </summary>
    public static FinancialModel FromInputParameters(ROIInputParameters input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var inputs = new FinancialInputs(
            input.ProjectBudget,
            input.NumberOfEmployees,
            input.ProjectDurationMonths);

        var assumptions = new FinancialAssumptions(
            input.ExpectedProductivityGain,
            input.BudgetLossRate,
            input.ProjectedRiskReduction,
            input.ExpectedSuccessBenefit);

        var risk = new RiskParameters(
            input.FailureRate,
            input.ExpectedDisengagementRate);

        return new FinancialModel(inputs, assumptions, risk);
    }
}
