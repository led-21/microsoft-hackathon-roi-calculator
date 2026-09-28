namespace microsoft_hackathon_roi_calculator.Domain.Financial;

/// <summary>
/// Deterministic financial calculation engine for ROI modeling.
/// Pure functions with zero external dependencies.
/// </summary>
public static class ROICalculator
{
    /// <summary>
    /// Executes the full financial calculation pipeline:
    /// FinancialModel -> Gross Calculations -> Risk Model Adjustments -> ROICalculationResult
    /// </summary>
    public static ROICalculationResult Calculate(FinancialModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var inputs = model.Inputs;
        var assumptions = model.Assumptions;
        var risk = model.Risk;

        // Step 1: Average Monthly Employee Cost
        double averageEmployeeCostPerMonth = inputs.ProjectBudget / (inputs.NumberOfEmployees * inputs.ProjectDurationMonths);

        // Step 2: Gross Benefits
        double monthlyProductivityGainPerEmployee = averageEmployeeCostPerMonth * (assumptions.ExpectedProductivityGain - 1.0);
        double productivityGainValue = monthlyProductivityGainPerEmployee * inputs.NumberOfEmployees * inputs.ProjectDurationMonths;

        double potentialLoss = inputs.ProjectBudget * assumptions.BudgetLossRate;
        double riskReductionValue = potentialLoss * assumptions.ProjectedRiskReduction;

        double successBenefitValue = inputs.ProjectBudget * assumptions.ExpectedSuccessBenefit;

        // Step 3: Risk Model Adjustments
        double adjustedProductivity = RiskModel.AdjustProductivity(productivityGainValue, risk.ExpectedDisengagementRate);
        double adjustedRiskReduction = RiskModel.AdjustRiskReduction(riskReductionValue, risk.FailureRate);
        double adjustedSuccessBenefit = RiskModel.AdjustSuccessBenefit(successBenefitValue, risk.FailureRate);

        // Step 4: Total Benefits & ROI Calculation
        double totalBenefits = adjustedProductivity + adjustedRiskReduction + adjustedSuccessBenefit;
        double totalInvestment = inputs.ProjectBudget;

        double roiPercentage = ((totalBenefits - totalInvestment) / totalInvestment) * 100.0;

        // Step 5: Break-even Failure Rate Calculation
        double benefitsSubjectToFailure = riskReductionValue + successBenefitValue;
        double breakEvenFailureRate = double.NaN;
        if (benefitsSubjectToFailure > 0)
        {
            breakEvenFailureRate = 1.0 - ((totalInvestment - adjustedProductivity) / benefitsSubjectToFailure);
            breakEvenFailureRate = Math.Clamp(breakEvenFailureRate, 0.0, 1.0);
        }

        var recommendations = GenerateRecommendations(roiPercentage, risk.FailureRate, risk.ExpectedDisengagementRate);

        return new ROICalculationResult
        {
            TotalInvestment = totalInvestment,
            TotalBenefits = totalBenefits,
            RoiPercentage = roiPercentage,
            AverageEmployeeCostPerMonth = averageEmployeeCostPerMonth,
            ProductivityGainValue = productivityGainValue,
            AdjustedProductivityGainValue = adjustedProductivity,
            RiskReductionValue = riskReductionValue,
            AdjustedRiskReduction = adjustedRiskReduction,
            SuccessBenefitValue = successBenefitValue,
            AdjustedSuccessBenefit = adjustedSuccessBenefit,
            BreakEvenFailureRate = breakEvenFailureRate,
            ActionableRecommendations = recommendations
        };
    }

    private static IReadOnlyList<string> GenerateRecommendations(double roiPercentage, double failureRate, double disengagementRate)
    {
        var list = new List<string>();

        if (roiPercentage > 50)
            list.Add("High projected return: Initiative demonstrates solid financial viability under current assumptions.");
        else if (roiPercentage > 0)
            list.Add("Moderate projected return: Monitor adoption risks closely to prevent margin erosion.");
        else
            list.Add("Negative projected return: Re-evaluate scope, budget allocation, or risk mitigation before proceeding.");

        if (failureRate > 0.5)
            list.Add($"High delivery risk ({failureRate:P0}): Prioritize phased milestones and change enablement to drive down execution risk.");

        if (disengagementRate > 0.1)
            list.Add($"Adoption resistance risk ({disengagementRate:P0}): Invest in structured training and communications to protect productivity gains.");

        return list;
    }
}
