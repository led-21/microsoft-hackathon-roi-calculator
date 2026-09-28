using microsoft_hackathon_roi_calculator.Domain.Financial;
using Xunit;

namespace microsoft_hackathon_roi_calculator.Tests;

public class RiskModelTests
{
    [Theory]
    [InlineData(10000, 0.0, 10000)]
    [InlineData(10000, 0.15, 8500)]
    [InlineData(10000, 0.50, 5000)]
    [InlineData(10000, 1.0, 0)]
    public void AdjustProductivity_ShouldScaleCorrectlyByDisengagement(
        double grossProductivity, double disengagementRate, double expectedAdjusted)
    {
        var actual = RiskModel.AdjustProductivity(grossProductivity, disengagementRate);
        Assert.Equal(expectedAdjusted, actual, precision: 2);
    }

    [Theory]
    [InlineData(50000, 0.0, 50000)]
    [InlineData(50000, 0.70, 15000)]
    [InlineData(50000, 1.0, 0)]
    public void AdjustRiskReduction_ShouldScaleCorrectlyByFailureRate(
        double grossRiskReduction, double failureRate, double expectedAdjusted)
    {
        var actual = RiskModel.AdjustRiskReduction(grossRiskReduction, failureRate);
        Assert.Equal(expectedAdjusted, actual, precision: 2);
    }

    [Theory]
    [InlineData(100000, 0.0, 100000)]
    [InlineData(100000, 0.30, 70000)]
    [InlineData(100000, 0.70, 30000)]
    [InlineData(100000, 1.0, 0)]
    public void AdjustSuccessBenefit_ShouldScaleCorrectlyByFailureRate(
        double grossSuccessBenefit, double failureRate, double expectedAdjusted)
    {
        var actual = RiskModel.AdjustSuccessBenefit(grossSuccessBenefit, failureRate);
        Assert.Equal(expectedAdjusted, actual, precision: 2);
    }
}
