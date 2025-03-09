using microsoft_hackathon_roi_calculator.Core.Interfaces;
using microsoft_hackathon_roi_calculator.Core.Models;

namespace microsoft_hackathon_roi_calculator.Core.Services;

public class ROICalculatorService : IROICalculatorService
{
    public ROICalculatorResult CalculateROI(ProjectInput input)
    {
        // Validações básicas
        if (input.ProjectBudget <= 0 || input.NumberOfEmployees <= 0 || input.ProjectDurationMonths <= 0)
        {
            throw new ArgumentException("Os valores de entrada devem ser positivos");
        }

        // Cálculo dos custos potenciais de falha
        double potentialFailureCost = input.ProjectBudget * input.BudgetLossRate * input.FailureRate;

        // Cálculo do custo de desengajamento
        double budgetPerEmployee = input.ProjectBudget / input.NumberOfEmployees;
        double disengagementCost = budgetPerEmployee * input.ExpectedDisengagementRate * input.NumberOfEmployees;

        // Cálculo dos benefícios
        // Ganho de produtividade (aplicado apenas aos funcionários engajados)
        double engagedEmployees = input.NumberOfEmployees * (1 - input.ExpectedDisengagementRate);
        double productivitySavings = budgetPerEmployee * input.ExpectedProductivityGain * engagedEmployees;

        // Economia com redução de risco (baseada no custo potencial de falha evitado)
        double riskReductionSavings = potentialFailureCost * input.ProjectedRiskReduction;

        // Investimento total
        // Inclui o orçamento do projeto, custos de mitigação de desengajamento e outros custos relacionados
        double mitigationCost = disengagementCost * 0.5; // Assume 50% do custo de desengajamento como mitigação
        double totalInvestment = input.ProjectBudget + mitigationCost;

        // Benefícios totais
        double totalBenefits = productivitySavings + riskReductionSavings;

        // Cálculo do ROI
        double roiPercentage = ((totalBenefits - totalInvestment) / totalInvestment) * 100;

        // Resultado final
        return new ROICalculatorResult
        {
            TotalInvestment = totalInvestment,
            TotalBenefits = totalBenefits,
            RoiPercentage = roiPercentage,
            PotentialFailureCost = potentialFailureCost,
            DisengagementCost = disengagementCost,
            ProductivitySavings = productivitySavings,
            RiskReductionSavings = riskReductionSavings
        };

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
        
        Análise de Riscos:
        - Custo Potencial de Falha: R$ {result.PotentialFailureCost:N2}
        - Custo de Desengajamento: R$ {result.DisengagementCost:N2}
        
        Benefícios Projetados:
        - Economia com Produtividade: R$ {result.ProductivitySavings:N2}
        - Economia com Redução de Risco: R$ {result.RiskReductionSavings:N2}
        
        Resultado Final:
        - Investimento Total: R$ {result.TotalInvestment:N2}
        - Benefícios Totais: R$ {result.TotalBenefits:N2}
        - ROI: {result.RoiPercentage:N2}%
        
        Interpretação: {(result.RoiPercentage > 0 ?
            "O projeto apresenta retorno positivo" :
            "O projeto apresenta risco de retorno negativo")}
        ";
    }

}