using microsoft_hackathon_roi_calculator.Application.UseCases;
using microsoft_hackathon_roi_calculator.Domain.Models;
using Xunit;

namespace microsoft_hackathon_roi_calculator.Tests;

public class ROICalculatorServiceTests
{
    private readonly ROICalculatorService _service = new();

    [Fact]
    public void CalculateROI_ShouldThrow_WhenInputsAreInvalid()
    {
        var invalidInput = new ROIInputParameters
        {
            ProjectBudget = 0,
            NumberOfEmployees = 10,
            ProjectDurationMonths = 6
        };

        Assert.Throws<ArgumentException>(() => _service.CalculateROI(invalidInput));
    }

    [Fact]
    public void CalculateROI_ShouldMatchLegacyFormulaResults()
    {
        var input = new ROIInputParameters
        {
            ProjectBudget = 100_000,
            NumberOfEmployees = 10,
            ProjectDurationMonths = 6,
            ExpectedProductivityGain = 1.20,
            ExpectedDisengagementRate = 0.10,
            BudgetLossRate = 0.30,
            ProjectedRiskReduction = 0.50,
            ExpectedSuccessBenefit = 0.40,
            FailureRate = 0.70
        };

        var result = _service.CalculateROI(input);

        Assert.Equal(100_000, result.TotalInvestment);
        Assert.Equal(34_500, result.TotalBenefits, precision: 2);
        Assert.Equal(-65.50, result.RoiPercentage, precision: 2);
    }

    [Fact]
    public void GenerateReport_ShouldContainKeyFinancialMetrics()
    {
        var input = new ROIInputParameters
        {
            ProjectBudget = 100_000,
            NumberOfEmployees = 10,
            ProjectDurationMonths = 6
        };
        var result = _service.CalculateROI(input);

        var report = _service.GenerateReport(result, input);

        Assert.NotNull(report);
        Assert.Contains("Relatório de ROI do Projeto", report);
        Assert.Contains("Investimento Total:", report);
        Assert.Contains("Retorno sobre o Investimento (ROI):", report);
    }

    [Fact]
    public void EstimateFailureRate_ShouldReturnPlausibleBoundedValue()
    {
        var input = new ROIInputParameters
        {
            ProjectBudget = 150_000,
            NumberOfEmployees = 20,
            ProjectDurationMonths = 12
        };

        var failureRate = _service.EstimateFailureRate(input);

        Assert.True(failureRate >= 0.05 && failureRate <= 0.95,
            $"Failure rate {failureRate} should be within [0.05, 0.95].");
    }
}
