using microsoft_hackathon_roi_calculator.Domain.Financial;
using Xunit;

namespace microsoft_hackathon_roi_calculator.Tests;

public class ROICalculatorTests
{
    [Fact]
    public void Calculate_Scenario1_StandardTransformationCase()
    {
        // Arrange
        var inputs = new FinancialInputs(projectBudget: 100_000, numberOfEmployees: 10, projectDurationMonths: 6);
        var assumptions = new FinancialAssumptions(
            expectedProductivityGain: 1.20, // 20% gain
            budgetLossRate: 0.30,           // 30% loss if failure
            projectedRiskReduction: 0.50,   // 50% mitigated
            expectedSuccessBenefit: 0.40    // 40% upside
        );
        var risk = new RiskParameters(
            failureRate: 0.70,              // 70% failure benchmark
            expectedDisengagementRate: 0.10 // 10% disengagement
        );
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Assert
        // Average Monthly Cost = 100,000 / (10 * 6) = 1,666.6667
        Assert.Equal(1666.67, result.AverageEmployeeCostPerMonth, precision: 2);

        // Gross Productivity = 100,000 * 0.20 = 20,000
        Assert.Equal(20_000, result.ProductivityGainValue, precision: 2);
        // Adjusted Productivity = 20,000 * (1 - 0.10) = 18,000
        Assert.Equal(18_000, result.AdjustedProductivityGainValue, precision: 2);

        // Gross Risk Reduction = 100,000 * 0.30 * 0.50 = 15,000
        Assert.Equal(15_000, result.RiskReductionValue, precision: 2);
        // Adjusted Risk Reduction = 15,000 * (1 - 0.70) = 4,500
        Assert.Equal(4_500, result.AdjustedRiskReduction, precision: 2);

        // Gross Success Benefit = 100,000 * 0.40 = 40,000
        Assert.Equal(40_000, result.SuccessBenefitValue, precision: 2);
        // Adjusted Success Benefit = 40,000 * (1 - 0.70) = 12,000
        Assert.Equal(12_000, result.AdjustedSuccessBenefit, precision: 2);

        // Total Benefits = 18,000 + 4,500 + 12,000 = 34,500
        Assert.Equal(34_500, result.TotalBenefits, precision: 2);
        Assert.Equal(100_000, result.TotalInvestment, precision: 2);

        // Net Benefit = 34,500 - 100,000 = -65,500
        Assert.Equal(-65_500, result.NetBenefit, precision: 2);

        // ROI% = (34,500 - 100,000) / 100,000 * 100 = -65.50%
        Assert.Equal(-65.50, result.RoiPercentage, precision: 2);
        Assert.False(result.IsViable);
    }

    [Fact]
    public void Calculate_Scenario2_HighPerformanceTransformationCase()
    {
        // Arrange
        var inputs = new FinancialInputs(projectBudget: 200_000, numberOfEmployees: 25, projectDurationMonths: 12);
        var assumptions = new FinancialAssumptions(
            expectedProductivityGain: 1.50, // 50% gain
            budgetLossRate: 0.20,
            projectedRiskReduction: 0.80,
            expectedSuccessBenefit: 1.50    // 150% upside
        );
        var risk = new RiskParameters(
            failureRate: 0.20,              // 20% failure
            expectedDisengagementRate: 0.05 // 5% disengagement
        );
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Gross Productivity = 200,000 * 0.50 = 100,000
        // Adjusted Productivity = 100,000 * 0.95 = 95,000
        Assert.Equal(95_000, result.AdjustedProductivityGainValue, precision: 2);

        // Gross Risk = 200,000 * 0.20 * 0.80 = 32,000
        // Adjusted Risk = 32,000 * 0.80 = 25,600
        Assert.Equal(25_600, result.AdjustedRiskReduction, precision: 2);

        // Gross Success = 200,000 * 1.50 = 300,000
        // Adjusted Success = 300,000 * 0.80 = 240,000
        Assert.Equal(240_000, result.AdjustedSuccessBenefit, precision: 2);

        // Total Benefits = 95,000 + 25,600 + 240,000 = 360,600
        Assert.Equal(360_600, result.TotalBenefits, precision: 2);

        // ROI% = (360,600 - 200,000) / 200,000 * 100 = 80.30%
        Assert.Equal(80.30, result.RoiPercentage, precision: 2);
        Assert.True(result.IsViable);
        Assert.Equal(1.803, result.BenefitCostRatio, precision: 3);
    }

    [Fact]
    public void Calculate_BoundaryScenario_FullFailureRate_ReducesNonProductivityBenefitsToZero()
    {
        // When failure rate is 100%, risk reduction and success benefits are 0.
        var inputs = new FinancialInputs(50_000, 5, 3);
        var assumptions = new FinancialAssumptions(1.20, 0.30, 0.50, 1.0);
        var risk = new RiskParameters(failureRate: 1.0, expectedDisengagementRate: 0.0);
        var model = new FinancialModel(inputs, assumptions, risk);

        var result = ROICalculator.Calculate(model);

        Assert.Equal(0, result.AdjustedRiskReduction);
        Assert.Equal(0, result.AdjustedSuccessBenefit);
        Assert.Equal(10_000, result.AdjustedProductivityGainValue, precision: 2);
        Assert.Equal(10_000, result.TotalBenefits, precision: 2);
    }

    [Fact]
    public void Calculate_BoundaryScenario_SmallestInputs_DoesNotDivideByZero()
    {
        var inputs = new FinancialInputs(1.0, 1, 1);
        var model = new FinancialModel(inputs);

        var result = ROICalculator.Calculate(model);

        Assert.False(double.IsNaN(result.RoiPercentage));
        Assert.False(double.IsInfinity(result.RoiPercentage));
        Assert.Equal(1.0, result.TotalInvestment);
    }
}
