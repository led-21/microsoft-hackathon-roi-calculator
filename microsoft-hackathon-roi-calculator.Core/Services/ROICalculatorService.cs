using microsoft_hackathon_roi_calculator.Core.Interfaces;
using microsoft_hackathon_roi_calculator.Core.Models;

namespace microsoft_hackathon_roi_calculator.Core.Services;

public class ROICalculatorService : IROICalculatorService
{
    public ROICalculatorResult CalculateROI(ProjectInput input)
    {
        // Validação básica dos parâmetros
        if (input.ProjectBudget <= 0 || input.NumberOfEmployees <= 0 || input.ProjectDurationMonths <= 0)
            throw new ArgumentException("Os valores de orçamento, funcionários e duração devem ser maiores que zero.");

        if (input.BudgetLossRate < 0 || input.FailureRate < 0 || input.ExpectedDisengagementRate < 0 ||
            input.ExpectedProductivityGain < 0 || input.ProjectedRiskReduction < 0 || input.SuccessBenefit < 0)
            throw new ArgumentException("Taxas e ganhos não podem ser negativos.");

        // Cálculo dos benefícios

        //Custo médio mensal por funcionário
        double averageEmployeeCostPerMonth = input.ProjectBudget / input.NumberOfEmployees / input.ProjectDurationMonths;

        // 1. Economia por Produtividade Aumentada
        double productivityGainValue = CalculateProductivityGain(input, averageEmployeeCostPerMonth);
        double adjustedProductivityGain = productivityGainValue * (1 - input.ExpectedDisengagementRate);

        // 2. Redução de Risco
        double riskReductionValue = CalculateRiskReduction(input);
        double adjustedRiskReduction = riskReductionValue * (1 - input.FailureRate);

        // 3. Benefício de Sucesso
        double successBenefitValue = CalculateSuccessBenefit(input);
        double adjustedSuccessBenefit = successBenefitValue * (1 - input.FailureRate);

        // Soma dos benefícios ajustados
        double totalBenefits = adjustedProductivityGain + adjustedRiskReduction + adjustedSuccessBenefit;

        // Calcular o Investimento Total (neste caso, apenas o orçamento do projeto)
        double totalInvestment = input.ProjectBudget;

        // Calcular o ROI
        double roiPercentage = ((totalBenefits - totalInvestment) / totalInvestment) * 100;

        // Resultado final
        return new ROICalculatorResult
        {
            TotalInvestment = totalInvestment,
            TotalBenefits = totalBenefits,
            RoiPercentage = roiPercentage,
            ProductivityGainValue = productivityGainValue,
            AdjustedProductivityGainValue = adjustedProductivityGain,
            RiskReductionValue = riskReductionValue,
            AdjustedRiskReduction = adjustedRiskReduction,
            SuccessBenefitValue = successBenefitValue,
            AdjustedSuccessBenefit = adjustedSuccessBenefit
        };
    }

    private double CalculateProductivityGain(ProjectInput input, double averageEmployeeCostPerMonth)
    {
        // Calcula o ganho de produtividade com base no custo dos funcionários, ganho esperado e duração
        double monthlyProductivityGainPerEmployee = averageEmployeeCostPerMonth * (input.ExpectedProductivityGain - 1); // Ganho acima do baseline (1)
        double totalProductivityGain = monthlyProductivityGainPerEmployee * input.NumberOfEmployees * input.ProjectDurationMonths;
        return totalProductivityGain;
    }

    private double CalculateRiskReduction(ProjectInput input)
    {
        // Calcula o valor do risco evitado (percentual do orçamento que seria perdido)
        double potentialLoss = input.ProjectBudget * input.BudgetLossRate;
        double riskReduction = potentialLoss * input.ProjectedRiskReduction;
        return riskReduction;
    }

    private double CalculateSuccessBenefit(ProjectInput input)
    {
        // Calcula o benefício do sucesso como um múltiplo do orçamento
        double successBenefit = input.ProjectBudget * input.SuccessBenefit;
        return successBenefit;
    }

    // Método para gerar relatório detalhado
    public string GenerateReport(ROICalculatorResult result, ProjectInput input)
    {
        return $@"
             Relatório de ROI do Projeto
             -------------------------
             Orçamento do Projeto: R$ {input.ProjectBudget:N2}
             Funcionários Impactados: {input.NumberOfEmployees}
             Duração: {input.ProjectDurationMonths} meses

             Parâmetros de Risco e Benefício:
             - Taxa de Falha do Projeto: {input.FailureRate:P2} ({input.FailureRate * 100:F2}%)
             - Taxa de Desengajamento Esperada: {input.ExpectedDisengagementRate:P2} ({input.ExpectedDisengagementRate * 100:F2}%)
             - Percentual de Perda Orçamentária em Caso de Falha: {input.BudgetLossRate:P2} ({input.BudgetLossRate * 100:F2}%)
             - Ganho de Produtividade Esperado: {input.ExpectedProductivityGain:F2}x
             - Redução de Risco Projetada: {input.ProjectedRiskReduction:P2} ({input.ProjectedRiskReduction * 100:F2}%)
             - Benefício de Sucesso: {input.SuccessBenefit:F2}x o orçamento

             Perdas em Caso de Falha:
             - Perda Orçamentária: R$ {(input.ProjectBudget * input.BudgetLossRate):N2}

             Perdas de Desengajamento Esperado:
             - Perda de Produtividade: R$ {(result.ProductivityGainValue * input.ExpectedDisengagementRate):N2}

             Detalhamento dos Benefícios Totais:
             - Ganho de Produtividade Ajustado: R$ {result.AdjustedProductivityGainValue:N2}
             - Redução de Risco Ajustada: R$ {result.AdjustedRiskReduction:N2}
             - Benefício de Sucesso Ajustado: R$ {result.AdjustedSuccessBenefit:N2}
             - Benefícios Totais: R$ {result.TotalBenefits:N2}

             Resultado Final:
             - Investimento Total: R$ {input.ProjectBudget:N2}
             - Retorno sobre Investimento (ROI): {result.RoiPercentage:F2}%
             -------------------------
             ";
    }
}