using microsoft_hackathon_roi_calculator.Domain.Financial;
using Xunit;

namespace microsoft_hackathon_roi_calculator.Tests;

public class FinancialModelTests
{
    [Theory]
    [InlineData(0, 10, 6)]
    [InlineData(-5000, 10, 6)]
    [InlineData(10000, 0, 6)]
    [InlineData(10000, -1, 6)]
    [InlineData(10000, 5, 0)]
    [InlineData(10000, 5, -2)]
    public void FinancialInputs_ShouldThrow_WhenValuesAreZeroOrNegative(double budget, int employees, int duration)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FinancialInputs(budget, employees, duration));
    }

    [Fact]
    public void FinancialInputs_ShouldInitialize_WhenValid()
    {
        var inputs = new FinancialInputs(50000, 15, 8);
        Assert.Equal(50000, inputs.ProjectBudget);
        Assert.Equal(15, inputs.NumberOfEmployees);
        Assert.Equal(8, inputs.ProjectDurationMonths);
    }

    [Theory]
    [InlineData(-0.1, 0.3, 0.5, 1.5)]
    [InlineData(1.2, -0.1, 0.5, 1.5)]
    [InlineData(1.2, 1.1, 0.5, 1.5)]
    [InlineData(1.2, 0.3, -0.1, 1.5)]
    [InlineData(1.2, 0.3, 1.5, 1.5)]
    [InlineData(1.2, 0.3, 0.5, -0.5)]
    public void FinancialAssumptions_ShouldThrow_WhenRatesAreOutOfBounds(
        double productivityGain, double lossRate, double riskReduction, double successBenefit)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FinancialAssumptions(productivityGain, lossRate, riskReduction, successBenefit));
    }

    [Theory]
    [InlineData(-0.1, 0.15)]
    [InlineData(1.1, 0.15)]
    [InlineData(0.7, -0.05)]
    [InlineData(0.7, 1.05)]
    public void RiskParameters_ShouldThrow_WhenRatesAreOutOfBounds(double failureRate, double disengagementRate)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new RiskParameters(failureRate, disengagementRate));
    }
}
