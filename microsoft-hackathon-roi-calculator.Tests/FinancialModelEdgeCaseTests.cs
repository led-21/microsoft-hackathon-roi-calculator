using microsoft_hackathon_roi_calculator.Domain.Financial;
using microsoft_hackathon_roi_calculator.Domain.Models;
using Xunit;

namespace microsoft_hackathon_roi_calculator.Tests;

/// <summary>
/// Edge-case and gap-coverage tests for <see cref="FinancialAssumptions"/>,
/// <see cref="RiskParameters"/>, <see cref="FinancialModel"/>,
/// <see cref="ROICalculationResult"/>, and <see cref="ROICalculator"/>
/// recommendation logic.
/// </summary>
public class FinancialModelEdgeCaseTests
{
    // ────────────────────────────────────────────────────────────────
    // 1. FinancialAssumptions — default constructor
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void FinancialAssumptions_DefaultConstructor_SetsExpectedDefaults()
    {
        // Arrange & Act
        var assumptions = new FinancialAssumptions();

        // Assert
        Assert.Equal(1.2, assumptions.ExpectedProductivityGain);
        Assert.Equal(0.3, assumptions.BudgetLossRate);
        Assert.Equal(0.0, assumptions.ProjectedRiskReduction);
        Assert.Equal(1.5, assumptions.ExpectedSuccessBenefit);
    }

    // ────────────────────────────────────────────────────────────────
    // 2. FinancialAssumptions — valid parameterized constructor
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void FinancialAssumptions_ParameterizedConstructor_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var assumptions = new FinancialAssumptions(
            expectedProductivityGain: 1.35,
            budgetLossRate: 0.45,
            projectedRiskReduction: 0.60,
            expectedSuccessBenefit: 2.0);

        // Assert
        Assert.Equal(1.35, assumptions.ExpectedProductivityGain);
        Assert.Equal(0.45, assumptions.BudgetLossRate);
        Assert.Equal(0.60, assumptions.ProjectedRiskReduction);
        Assert.Equal(2.0, assumptions.ExpectedSuccessBenefit);
    }

    [Theory]
    [InlineData(0.0, 0.0, 0.0, 0.0)]   // all-zero lower boundary
    [InlineData(0.0, 1.0, 1.0, 0.0)]   // rates at upper boundary
    public void FinancialAssumptions_BoundaryValues_DoNotThrow(
        double productivityGain, double lossRate, double riskReduction, double successBenefit)
    {
        // Arrange & Act
        var assumptions = new FinancialAssumptions(productivityGain, lossRate, riskReduction, successBenefit);

        // Assert
        Assert.Equal(productivityGain, assumptions.ExpectedProductivityGain);
        Assert.Equal(lossRate, assumptions.BudgetLossRate);
        Assert.Equal(riskReduction, assumptions.ProjectedRiskReduction);
        Assert.Equal(successBenefit, assumptions.ExpectedSuccessBenefit);
    }

    // ────────────────────────────────────────────────────────────────
    // 3. RiskParameters — default constructor
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void RiskParameters_DefaultConstructor_SetsExpectedDefaults()
    {
        // Arrange & Act
        var risk = new RiskParameters();

        // Assert
        Assert.Equal(0.7, risk.FailureRate);
        Assert.Equal(0.15, risk.ExpectedDisengagementRate);
    }

    // ────────────────────────────────────────────────────────────────
    // 4. RiskParameters — valid parameterized constructor at boundaries
    // ────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(1.0, 1.0)]
    [InlineData(0.0, 1.0)]
    [InlineData(1.0, 0.0)]
    public void RiskParameters_BoundaryValues_DoNotThrow(double failureRate, double disengagementRate)
    {
        // Arrange & Act
        var risk = new RiskParameters(failureRate, disengagementRate);

        // Assert
        Assert.Equal(failureRate, risk.FailureRate);
        Assert.Equal(disengagementRate, risk.ExpectedDisengagementRate);
    }

    // ────────────────────────────────────────────────────────────────
    // 5. FinancialModel.FromInputParameters(null) — ArgumentNullException
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void FromInputParameters_NullInput_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => FinancialModel.FromInputParameters(null!));
    }

    // ────────────────────────────────────────────────────────────────
    // 6. FinancialModel constructor — null FinancialInputs
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void FinancialModel_NullInputs_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FinancialModel(inputs: null!));
    }

    // ────────────────────────────────────────────────────────────────
    // 7. FinancialModel — null assumptions / risk uses defaults
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void FinancialModel_NullAssumptionsAndRisk_UsesDefaults()
    {
        // Arrange
        var inputs = new FinancialInputs(100_000, 10, 6);

        // Act
        var model = new FinancialModel(inputs, assumptions: null, risk: null);

        // Assert — defaults come from parameterless constructors
        Assert.Equal(1.2, model.Assumptions.ExpectedProductivityGain);
        Assert.Equal(0.3, model.Assumptions.BudgetLossRate);
        Assert.Equal(0.0, model.Assumptions.ProjectedRiskReduction);
        Assert.Equal(1.5, model.Assumptions.ExpectedSuccessBenefit);

        Assert.Equal(0.7, model.Risk.FailureRate);
        Assert.Equal(0.15, model.Risk.ExpectedDisengagementRate);
    }

    // ────────────────────────────────────────────────────────────────
    // 8. BreakEvenFailureRate — verified against hand-calculation
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void Calculate_BreakEvenFailureRate_MatchesHandCalculation()
    {
        // Arrange — scenario where benefitsSubjectToFailure > 0
        // Budget = 100,000; Employees = 10; Duration = 6
        // Assumptions: productivityGain=1.20, lossRate=0.30, riskReduction=0.50, successBenefit=0.40
        // Risk: failureRate=0.70, disengagementRate=0.10
        //
        // Gross Productivity           = 100,000 * 0.20 = 20,000
        // Adjusted Productivity         = 20,000 * 0.90 = 18,000
        // Gross Risk Reduction          = 100,000 * 0.30 * 0.50 = 15,000
        // Gross Success Benefit         = 100,000 * 0.40 = 40,000
        //
        // benefitsSubjectToFailure      = 15,000 + 40,000 = 55,000
        // breakEven = 1.0 - (100,000 - 18,000) / 55,000
        //           = 1.0 - 82,000 / 55,000
        //           = 1.0 - 1.49091…
        //           = -0.49091… → clamped to 0.0
        var inputs = new FinancialInputs(100_000, 10, 6);
        var assumptions = new FinancialAssumptions(1.20, 0.30, 0.50, 0.40);
        var risk = new RiskParameters(0.70, 0.10);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Assert — clamped to 0.0
        Assert.Equal(0.0, result.BreakEvenFailureRate, precision: 4);
    }

    [Fact]
    public void Calculate_BreakEvenFailureRate_PositiveValue_WhenBenefitsAreLarge()
    {
        // Arrange — larger success benefit so numerator stays positive
        // Budget = 200,000; Employees = 25; Duration = 12
        // Assumptions: productivityGain=1.50, lossRate=0.20, riskReduction=0.80, successBenefit=1.50
        // Risk: failureRate=0.20, disengagementRate=0.05
        //
        // Gross Productivity      = 200,000 * 0.50 = 100,000
        // Adjusted Productivity    = 100,000 * 0.95 = 95,000
        // Gross Risk Reduction     = 200,000 * 0.20 * 0.80 = 32,000
        // Gross Success Benefit    = 200,000 * 1.50 = 300,000
        //
        // benefitsSubjectToFailure = 32,000 + 300,000 = 332,000
        // breakEven = 1.0 - (200,000 - 95,000) / 332,000
        //           = 1.0 - 105,000 / 332,000
        //           = 1.0 - 0.31627…
        //           = 0.68373…
        var inputs = new FinancialInputs(200_000, 25, 12);
        var assumptions = new FinancialAssumptions(1.50, 0.20, 0.80, 1.50);
        var risk = new RiskParameters(0.20, 0.05);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Assert
        Assert.Equal(0.6837, result.BreakEvenFailureRate, precision: 4);
    }

    [Fact]
    public void Calculate_BreakEvenFailureRate_IsNaN_WhenBenefitsSubjectToFailureAreZero()
    {
        // Arrange — riskReduction=0, successBenefit=0 → benefitsSubjectToFailure = 0
        var inputs = new FinancialInputs(50_000, 5, 3);
        var assumptions = new FinancialAssumptions(1.20, 0.0, 0.0, 0.0);
        var risk = new RiskParameters(0.50, 0.10);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Assert
        Assert.True(double.IsNaN(result.BreakEvenFailureRate));
    }

    // ────────────────────────────────────────────────────────────────
    // 9. ToLegacyModel — field mapping
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void ToLegacyModel_MapsAllFieldsCorrectly()
    {
        // Arrange
        var result = new ROICalculationResult
        {
            TotalInvestment = 100_000,
            TotalBenefits = 34_500,
            RoiPercentage = -65.50,
            ProductivityGainValue = 20_000,
            AdjustedProductivityGainValue = 18_000,
            RiskReductionValue = 15_000,
            AdjustedRiskReduction = 4_500,
            SuccessBenefitValue = 40_000,
            AdjustedSuccessBenefit = 12_000,
            BreakEvenFailureRate = 0.0,
            ActionableRecommendations = new[] { "Recommendation A", "Recommendation B" }
        };

        // Act
        ROICalculationResults legacy = result.ToLegacyModel();

        // Assert
        Assert.Equal(result.TotalInvestment, legacy.TotalInvestment);
        Assert.Equal(result.TotalBenefits, legacy.TotalBenefits);
        Assert.Equal(result.RoiPercentage, legacy.RoiPercentage);
        Assert.Equal(result.ProductivityGainValue, legacy.ProductivityGainValue);
        Assert.Equal(result.AdjustedProductivityGainValue, legacy.AdjustedProductivityGainValue);
        Assert.Equal(result.RiskReductionValue, legacy.RiskReductionValue);
        Assert.Equal(result.AdjustedRiskReduction, legacy.AdjustedRiskReduction);
        Assert.Equal(result.SuccessBenefitValue, legacy.SuccessBenefitValue);
        Assert.Equal(result.AdjustedSuccessBenefit, legacy.AdjustedSuccessBenefit);
        Assert.Equal(result.ActionableRecommendations, legacy.ActionableRecommendations);
    }

    [Fact]
    public void ToLegacyModel_RecommendationsAreSeparateList()
    {
        // Verify ToLegacyModel creates an independent List<string>, not a shared reference.
        var result = new ROICalculationResult
        {
            ActionableRecommendations = new[] { "Keep going" }
        };

        var legacy = result.ToLegacyModel();

        // Mutate legacy list — should not affect the original result
        legacy.ActionableRecommendations.Add("Extra");

        Assert.Single(result.ActionableRecommendations);
        Assert.Equal(2, legacy.ActionableRecommendations.Count);
    }

    // ────────────────────────────────────────────────────────────────
    // 10. ROICalculator recommendation branches by ROI %
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void Calculate_Recommendation_HighROI_ContainsHighReturnMessage()
    {
        // Arrange — engineer ROI > 50%
        var inputs = new FinancialInputs(200_000, 25, 12);
        var assumptions = new FinancialAssumptions(1.50, 0.20, 0.80, 1.50);
        var risk = new RiskParameters(0.20, 0.05);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Pre-condition: ROI > 50%
        Assert.True(result.RoiPercentage > 50);
        Assert.Contains(result.ActionableRecommendations,
            r => r.StartsWith("High projected return"));
    }

    [Fact]
    public void Calculate_Recommendation_ModerateROI_ContainsModerateReturnMessage()
    {
        // Arrange — engineer 0 < ROI <= 50%
        // Budget = 100,000; productivityGain = 1.40 (40%); lossRate=0.30; riskReduction=0.50; successBenefit=0.80
        // failureRate=0.30; disengagementRate=0.05
        //
        // Gross Productivity  = 100,000 * 0.40 = 40,000   → Adjusted = 40,000 * 0.95 = 38,000
        // Gross RiskReduction = 100,000 * 0.30 * 0.50 = 15,000 → Adjusted = 15,000 * 0.70 = 10,500
        // Gross SuccessBenefit= 100,000 * 0.80 = 80,000   → Adjusted = 80,000 * 0.70 = 56,000
        // Total Benefits = 38,000 + 10,500 + 56,000 = 104,500
        // ROI = (104,500 - 100,000) / 100,000 * 100 = 4.50%
        var inputs = new FinancialInputs(100_000, 10, 6);
        var assumptions = new FinancialAssumptions(1.40, 0.30, 0.50, 0.80);
        var risk = new RiskParameters(0.30, 0.05);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Pre-condition: 0 < ROI <= 50
        Assert.True(result.RoiPercentage > 0, $"Expected ROI > 0 but was {result.RoiPercentage}");
        Assert.True(result.RoiPercentage <= 50, $"Expected ROI <= 50 but was {result.RoiPercentage}");
        Assert.Contains(result.ActionableRecommendations,
            r => r.StartsWith("Moderate projected return"));
    }

    [Fact]
    public void Calculate_Recommendation_NegativeROI_ContainsNegativeReturnMessage()
    {
        // Arrange — engineer ROI <= 0%
        var inputs = new FinancialInputs(100_000, 10, 6);
        var assumptions = new FinancialAssumptions(1.20, 0.30, 0.50, 0.40);
        var risk = new RiskParameters(0.70, 0.10);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Pre-condition: ROI <= 0
        Assert.True(result.RoiPercentage <= 0, $"Expected ROI <= 0 but was {result.RoiPercentage}");
        Assert.Contains(result.ActionableRecommendations,
            r => r.StartsWith("Negative projected return"));
    }

    // ────────────────────────────────────────────────────────────────
    // 11. Recommendation branches: failureRate > 0.5, disengagementRate > 0.1
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void Calculate_Recommendation_HighFailureRate_ContainsDeliveryRiskWarning()
    {
        // Arrange — failureRate = 0.80 (> 0.5)
        var inputs = new FinancialInputs(100_000, 10, 6);
        var assumptions = new FinancialAssumptions(1.20, 0.30, 0.50, 0.40);
        var risk = new RiskParameters(0.80, 0.05);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Assert
        Assert.Contains(result.ActionableRecommendations,
            r => r.Contains("High delivery risk"));
    }

    [Fact]
    public void Calculate_Recommendation_LowFailureRate_DoesNotContainDeliveryRiskWarning()
    {
        // Arrange — failureRate = 0.30 (<= 0.5)
        var inputs = new FinancialInputs(100_000, 10, 6);
        var assumptions = new FinancialAssumptions(1.20, 0.30, 0.50, 0.40);
        var risk = new RiskParameters(0.30, 0.05);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Assert
        Assert.DoesNotContain(result.ActionableRecommendations,
            r => r.Contains("High delivery risk"));
    }

    [Fact]
    public void Calculate_Recommendation_HighDisengagement_ContainsAdoptionResistanceWarning()
    {
        // Arrange — disengagementRate = 0.25 (> 0.1)
        var inputs = new FinancialInputs(100_000, 10, 6);
        var assumptions = new FinancialAssumptions(1.20, 0.30, 0.50, 0.40);
        var risk = new RiskParameters(0.30, 0.25);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Assert
        Assert.Contains(result.ActionableRecommendations,
            r => r.Contains("Adoption resistance risk"));
    }

    [Fact]
    public void Calculate_Recommendation_LowDisengagement_DoesNotContainAdoptionResistanceWarning()
    {
        // Arrange — disengagementRate = 0.05 (<= 0.1)
        var inputs = new FinancialInputs(100_000, 10, 6);
        var assumptions = new FinancialAssumptions(1.20, 0.30, 0.50, 0.40);
        var risk = new RiskParameters(0.30, 0.05);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Assert
        Assert.DoesNotContain(result.ActionableRecommendations,
            r => r.Contains("Adoption resistance risk"));
    }

    [Fact]
    public void Calculate_Recommendation_HighFailureAndHighDisengagement_ContainsBothWarnings()
    {
        // Arrange — failureRate = 0.80 (> 0.5) AND disengagementRate = 0.25 (> 0.1)
        var inputs = new FinancialInputs(100_000, 10, 6);
        var assumptions = new FinancialAssumptions(1.20, 0.30, 0.50, 0.40);
        var risk = new RiskParameters(0.80, 0.25);
        var model = new FinancialModel(inputs, assumptions, risk);

        // Act
        var result = ROICalculator.Calculate(model);

        // Assert — should have the ROI recommendation + both risk warnings = 3 items
        Assert.True(result.ActionableRecommendations.Count >= 3,
            $"Expected at least 3 recommendations but got {result.ActionableRecommendations.Count}");
        Assert.Contains(result.ActionableRecommendations,
            r => r.Contains("High delivery risk"));
        Assert.Contains(result.ActionableRecommendations,
            r => r.Contains("Adoption resistance risk"));
    }

    [Fact]
    public void Calculate_Recommendation_ExactBoundary_FailureRateAt50Percent_NoDeliveryRiskWarning()
    {
        // failureRate == 0.5 is NOT > 0.5, so no warning
        var inputs = new FinancialInputs(100_000, 10, 6);
        var assumptions = new FinancialAssumptions(1.20, 0.30, 0.50, 0.40);
        var risk = new RiskParameters(0.50, 0.05);
        var model = new FinancialModel(inputs, assumptions, risk);

        var result = ROICalculator.Calculate(model);

        Assert.DoesNotContain(result.ActionableRecommendations,
            r => r.Contains("High delivery risk"));
    }

    [Fact]
    public void Calculate_Recommendation_ExactBoundary_DisengagementAt10Percent_NoAdoptionWarning()
    {
        // disengagementRate == 0.1 is NOT > 0.1, so no warning
        var inputs = new FinancialInputs(100_000, 10, 6);
        var assumptions = new FinancialAssumptions(1.20, 0.30, 0.50, 0.40);
        var risk = new RiskParameters(0.30, 0.10);
        var model = new FinancialModel(inputs, assumptions, risk);

        var result = ROICalculator.Calculate(model);

        Assert.DoesNotContain(result.ActionableRecommendations,
            r => r.Contains("Adoption resistance risk"));
    }

    // ────────────────────────────────────────────────────────────────
    // Bonus: FromInputParameters maps all fields correctly
    // ────────────────────────────────────────────────────────────────

    [Fact]
    public void FromInputParameters_MapsAllFieldsToFinancialModel()
    {
        // Arrange
        var input = new ROIInputParameters
        {
            ProjectBudget = 150_000,
            NumberOfEmployees = 20,
            ProjectDurationMonths = 10,
            ExpectedProductivityGain = 1.30,
            BudgetLossRate = 0.25,
            ProjectedRiskReduction = 0.60,
            ExpectedSuccessBenefit = 1.00,
            FailureRate = 0.40,
            ExpectedDisengagementRate = 0.12
        };

        // Act
        var model = FinancialModel.FromInputParameters(input);

        // Assert — inputs
        Assert.Equal(150_000, model.Inputs.ProjectBudget);
        Assert.Equal(20, model.Inputs.NumberOfEmployees);
        Assert.Equal(10, model.Inputs.ProjectDurationMonths);

        // Assert — assumptions
        Assert.Equal(1.30, model.Assumptions.ExpectedProductivityGain);
        Assert.Equal(0.25, model.Assumptions.BudgetLossRate);
        Assert.Equal(0.60, model.Assumptions.ProjectedRiskReduction);
        Assert.Equal(1.00, model.Assumptions.ExpectedSuccessBenefit);

        // Assert — risk
        Assert.Equal(0.40, model.Risk.FailureRate);
        Assert.Equal(0.12, model.Risk.ExpectedDisengagementRate);
    }
}
