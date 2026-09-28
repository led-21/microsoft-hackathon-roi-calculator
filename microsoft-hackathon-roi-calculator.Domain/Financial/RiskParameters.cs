namespace microsoft_hackathon_roi_calculator.Domain.Financial;

/// <summary>
/// Organizational and execution risk parameters.
/// </summary>
/// <param name="FailureRate">
/// Probability of project delivery failure (e.g. 0.7 for standard transformation benchmark of 70% failure).
/// </param>
/// <param name="ExpectedDisengagementRate">
/// Estimated proportion of productivity gains degraded due to employee disengagement or change resistance (e.g. 0.15 for 15%).
/// </param>
public record RiskParameters
{
    public double FailureRate { get; init; } = 0.7;
    public double ExpectedDisengagementRate { get; init; } = 0.15;

    public RiskParameters() { }

    public RiskParameters(double failureRate, double expectedDisengagementRate)
    {
        if (failureRate < 0 || failureRate > 1.0)
            throw new ArgumentOutOfRangeException(nameof(failureRate), "Failure rate must be between 0.0 and 1.0.");
        if (expectedDisengagementRate < 0 || expectedDisengagementRate > 1.0)
            throw new ArgumentOutOfRangeException(nameof(expectedDisengagementRate), "Disengagement rate must be between 0.0 and 1.0.");

        FailureRate = failureRate;
        ExpectedDisengagementRate = expectedDisengagementRate;
    }
}
