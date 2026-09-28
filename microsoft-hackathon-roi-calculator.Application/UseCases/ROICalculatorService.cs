using System.Globalization;
using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Domain.Financial;
using microsoft_hackathon_roi_calculator.Domain.Models;
using Microsoft_hackathon_roi_calculator_Application;

namespace microsoft_hackathon_roi_calculator.Application.UseCases;

/// <summary>
/// Application service implementing ROI calculation use cases, ML failure rate estimation, and Markdown reporting.
/// </summary>
public class ROICalculatorService : IROICalculatorService
{
    private static readonly CultureInfo BrazilianCulture = new("pt-BR");

    public ROICalculationResult Calculate(FinancialModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return ROICalculator.Calculate(model);
    }

    public ROICalculationResults CalculateROI(ROIInputParameters input)
    {
        ArgumentNullException.ThrowIfNull(input);

        // Validation for negative or zero values
        if (input.ProjectBudget <= 0 || input.NumberOfEmployees <= 0 || input.ProjectDurationMonths <= 0)
            throw new ArgumentException("Budget, employees and duration must be greater than zero.");

        if (input.BudgetLossRate < 0 || input.FailureRate < 0 || input.ExpectedDisengagementRate < 0 ||
            input.ExpectedProductivityGain < 0 || input.ProjectedRiskReduction < 0 || input.ExpectedSuccessBenefit < 0)
            throw new ArgumentException("Rates and multipliers cannot be negative.");

        var model = FinancialModel.FromInputParameters(input);
        var result = ROICalculator.Calculate(model);
        return result.ToLegacyModel();
    }

    public double EstimateFailureRate(ROIInputParameters input)
    {
        ArgumentNullException.ThrowIfNull(input);

        try
        {
            var model = FinancialModel.FromInputParameters(input);

            double predictedROI = MLModel.Predict(new MLModel.ModelInput
            {
                ProjectBudget = (float)input.ProjectBudget,
                NumberOfEmployees = input.NumberOfEmployees,
                ProjectDurationMonths = input.ProjectDurationMonths,
                ROI = 0
            }).Score;

            double grossProductivity = model.Inputs.ProjectBudget * (model.Assumptions.ExpectedProductivityGain - 1.0);
            double adjustedProductivity = RiskModel.AdjustProductivity(grossProductivity, model.Risk.ExpectedDisengagementRate);

            double grossRisk = model.Inputs.ProjectBudget * model.Assumptions.BudgetLossRate * model.Assumptions.ProjectedRiskReduction;
            double grossSuccess = model.Inputs.ProjectBudget * model.Assumptions.ExpectedSuccessBenefit;

            double benefitsToAdjust = grossRisk + grossSuccess;
            if (benefitsToAdjust <= 0)
                return 0.50;

            double totalBenefits = (predictedROI * model.Inputs.ProjectBudget) + model.Inputs.ProjectBudget;
            double failureRate = (benefitsToAdjust - totalBenefits + adjustedProductivity) / benefitsToAdjust;

            if (double.IsNaN(failureRate) || double.IsInfinity(failureRate))
                return 0.50;

            return Math.Clamp(failureRate, 0.05, 0.95);
        }
        catch
        {
            return 0.50;
        }
    }

    public string GenerateReport(ROICalculationResults result, ROIInputParameters input)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(input);

        double averageCostPerEmployeeMonth = (input.NumberOfEmployees > 0 && input.ProjectDurationMonths > 0)
            ? input.ProjectBudget / (input.NumberOfEmployees * input.ProjectDurationMonths)
            : 0;

        string viability = result.RoiPercentage > 0 ? "Positiva (Viável)" : "Negativa (Atenção / Risco Elevado)";

        return $"""
## Relatório de ROI do Projeto — InovaROI
--------------------------------------------------

### 1. Parâmetros do Projeto (Inputs)
- **Orçamento Total:** {input.ProjectBudget.ToString("C", BrazilianCulture)}
- **Colaboradores Impactados:** {input.NumberOfEmployees}
- **Duração Estimada:** {input.ProjectDurationMonths} meses
- **Custo Médio Mensal por Colaborador:** {averageCostPerEmployeeMonth.ToString("C", BrazilianCulture)}

### 2. Premissas e Fatores de Risco
- **Probabilidade de Falha (Benchmark):** {input.FailureRate:P1}
- **Perda Estimada por Desengajamento:** {input.ExpectedDisengagementRate:P1}
- **Risco Orçamentário em Caso de Falha:** {input.BudgetLossRate:P1}
- **Multiplicador de Produtividade Esperado:** {input.ExpectedProductivityGain:F2}x
- **Mitigação Projetada de Risco:** {input.ProjectedRiskReduction:P1}
- **Upside de Sucesso:** {input.ExpectedSuccessBenefit:F2}x o orçamento

### 3. Detalhamento dos Benefícios Ajustados por Risco

#### A. Ganho de Produtividade:
- **Ganho Bruto:** {result.ProductivityGainValue.ToString("C", BrazilianCulture)}
- **Ajuste por Desengajamento (-{input.ExpectedDisengagementRate:P0}):** -{(result.ProductivityGainValue * input.ExpectedDisengagementRate).ToString("C", BrazilianCulture)}
- **Ganho Ajustado:** {result.AdjustedProductivityGainValue.ToString("C", BrazilianCulture)}

#### B. Redução de Risco:
- **Redução Bruta:** {result.RiskReductionValue.ToString("C", BrazilianCulture)}
- **Ajuste por Taxa de Falha (-{input.FailureRate:P0}):** -{(result.RiskReductionValue * input.FailureRate).ToString("C", BrazilianCulture)}
- **Redução Ajustada:** {result.AdjustedRiskReduction.ToString("C", BrazilianCulture)}

#### C. Benefício de Sucesso:
- **Benefício Bruto:** {result.SuccessBenefitValue.ToString("C", BrazilianCulture)}
- **Ajuste por Taxa de Falha (-{input.FailureRate:P0}):** -{(result.SuccessBenefitValue * input.FailureRate).ToString("C", BrazilianCulture)}
- **Benefício Ajustado:** {result.AdjustedSuccessBenefit.ToString("C", BrazilianCulture)}

---

### 4. Resultado Consolidado
- **Investimento Total:** {result.TotalInvestment.ToString("C", BrazilianCulture)}
- **Benefícios Totais Ajustados:** {result.TotalBenefits.ToString("C", BrazilianCulture)}
- **Retorno sobre o Investimento (ROI):** **{result.RoiPercentage:F2}%**
- **Diagnóstico de Viabilidade:** **{viability}**
--------------------------------------------------
""";
    }
}