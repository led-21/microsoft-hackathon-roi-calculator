namespace microsoft_hackathon_roi_calculator.Domain.Financial;

/// <summary>
/// Domain model that applies organizational and failure risks to raw financial benefits.
/// </summary>
public static class RiskModel
{
    /// <summary>
    /// Adjusts gross productivity gains by the expected employee disengagement rate.
    /// Formula: AdjustedProductivity = GrossProductivity * (1 - DisengagementRate)
    /// </summary>
    public static double AdjustProductivity(double grossProductivity, double disengagementRate)
    {
        var retentionRate = Math.Clamp(1.0 - disengagementRate, 0.0, 1.0);
        return grossProductivity * retentionRate;
    }

    /// <summary>
    /// Adjusts gross risk mitigation savings by the project failure rate.
    /// If the project fails, the mitigation cannot be realized.
    /// Formula: AdjustedRiskReduction = GrossRiskReduction * (1 - FailureRate)
    /// </summary>
    public static double AdjustRiskReduction(double grossRiskReduction, double failureRate)
    {
        var successProbability = Math.Clamp(1.0 - failureRate, 0.0, 1.0);
        return grossRiskReduction * successProbability;
    }

    /// <summary>
    /// Adjusts gross success upside by the project failure rate.
    /// Upside benefits are realized only upon successful delivery.
    /// Formula: AdjustedSuccessBenefit = GrossSuccessBenefit * (1 - FailureRate)
    /// </summary>
    public static double AdjustSuccessBenefit(double grossSuccessBenefit, double failureRate)
    {
        var successProbability = Math.Clamp(1.0 - failureRate, 0.0, 1.0);
        return grossSuccessBenefit * successProbability;
    }
}
