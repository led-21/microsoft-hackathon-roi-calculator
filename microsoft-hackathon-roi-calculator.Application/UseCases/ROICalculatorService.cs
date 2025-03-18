using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Domain.Models;
using Microsoft_hackathon_roi_calculator_Application;

namespace microsoft_hackathon_roi_calculator.Application.UseCases;

public class ROICalculatorService : IROICalculatorService
{
    public ROICalculationResults CalculateROI(ROIInputParameters input)
    {
        // Validação básica dos parâmetros
        if (input.ProjectBudget <= 0 || input.NumberOfEmployees <= 0 || input.ProjectDurationMonths <= 0)
            throw new ArgumentException("Os valores de orçamento, funcionários e duração devem ser maiores que zero.");

        if (input.BudgetLossRate < 0 || input.FailureRate < 0 || input.ExpectedDisengagementRate < 0 ||
            input.ExpectedProductivityGain < 0 || input.ProjectedRiskReduction < 0 || input.ExpectedSuccessBenefit < 0)
            throw new ArgumentException("Taxas e ganhos não podem ser negativos.");

        // Cálculo dos benefícios

        //Custo médio mensal por funcionário
        double averageEmployeeCostPerMonth = CalculateAverageEmployeeCostPerMonth(input);

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
        double roiPercentage = (totalBenefits - totalInvestment) / totalInvestment * 100;

        // Resultado final
        return new ROICalculationResults
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

    private double CalculateAverageEmployeeCostPerMonth(ROIInputParameters input)
    {
        return input.ProjectBudget / input.NumberOfEmployees / input.ProjectDurationMonths;
    }

    private double CalculateProductivityGain(ROIInputParameters input, double averageEmployeeCostPerMonth)
    {
        // Calcula o ganho de produtividade com base no custo dos funcionários, ganho esperado e duração
        double monthlyProductivityGainPerEmployee = averageEmployeeCostPerMonth * (input.ExpectedProductivityGain - 1); // Ganho acima do baseline (1)
        double totalProductivityGain = monthlyProductivityGainPerEmployee * input.NumberOfEmployees * input.ProjectDurationMonths;
        return totalProductivityGain;
    }

    private double CalculateRiskReduction(ROIInputParameters input)
    {
        // Calcula o valor do risco evitado (percentual do orçamento que seria perdido)
        double potentialLoss = input.ProjectBudget * input.BudgetLossRate;
        double riskReduction = potentialLoss * input.ProjectedRiskReduction;
        return riskReduction;
    }

    private double CalculateSuccessBenefit(ROIInputParameters input)
    {
        // Calcula o benefício do sucesso como um múltiplo do orçamento
        double successBenefit = input.ProjectBudget * input.ExpectedSuccessBenefit;
        return successBenefit;
    }

    public double EstimateFailureRate(ROIInputParameters input)
    {
        double expectedROIValue = MLModel.Predict(new MLModel.ModelInput
        {
            ProjectBudget = (float)input.ProjectBudget,
            NumberOfEmployees = input.NumberOfEmployees,
            ProjectDurationMonths = input.ProjectDurationMonths,
            ROI = 0
        }).Score;

        double averageEmployeeCostPerMonth = CalculateAverageEmployeeCostPerMonth(input);

        double productivityGainValue = CalculateProductivityGain(input, averageEmployeeCostPerMonth);
        double adjustedProductivityGain = productivityGainValue * (1 - input.ExpectedDisengagementRate);

        double riskReduction = CalculateRiskReduction(input);
        double successBenefit = CalculateSuccessBenefit(input);

        double totalBenefits = expectedROIValue * input.ProjectBudget + input.ProjectBudget;
        double benefitsToAdjust = riskReduction + successBenefit;

        double failureRate = (benefitsToAdjust - totalBenefits + adjustedProductivityGain) / benefitsToAdjust;

        return failureRate;
    }

    // Método para gerar relatório detalhado
    public string GenerateReport(ROICalculationResults result, ROIInputParameters input)
    {
        return $@"
## Relatório de ROI do Projeto
-------------------------

*Orçamento do Projeto:* R$ {input.ProjectBudget:N2}  
*Funcionários Impactados:* {input.NumberOfEmployees}  
*Duração:* {input.ProjectDurationMonths} meses  

### Parâmetros de Risco e Benefício:
- *Taxa de Falha do Projeto:* {input.FailureRate:P2}  
- *Taxa de Desengajamento Esperada:* {input.ExpectedDisengagementRate:P2}  
- *Percentual de Perda Orçamentária em Caso de Falha:* {input.BudgetLossRate:P2}  
- *Ganho de Produtividade Esperado:* {input.ExpectedProductivityGain:F2}x  
- *Redução de Risco Projetada:* {input.ProjectedRiskReduction:P2}  
- *Benefício de Sucesso:* {input.ExpectedSuccessBenefit:F2}x o orçamento  

#### Detalhamento dos Cálculos:
- *Custo Médio Mensal por Funcionário:* R$ {input.ProjectBudget / input.      NumberOfEmployees /           input.ProjectDurationMonths:N2}  

### Passos para Calcular os Valores Ajustados:

#### 1. Ganho de Produtividade:
- *Ganho de Produtividade Inicial:* R$ {result.ProductivityGainValue:N2}  
- *Ajuste por Desengajamento:* R$ {result.ProductivityGainValue *             input.ExpectedDisengagementRate:N2}  
- *Ganho de Produtividade Ajustado:* R$ {result.AdjustedProductivityGainValue:N2}  

#### 2. Redução de Risco:
- *Valor de Redução de Risco Inicial:* R$ {result.RiskReductionValue:N2}  
- *Ajuste por Taxa de Falha:* R$ {result.RiskReductionValue * input.FailureRate:N2}  
- *Redução de Risco Ajustada:* R$ {result.AdjustedRiskReduction:N2}  

#### 3. Benefício de Sucesso:
- *Valor de Benefício de Sucesso Inicial:* R$ {result.SuccessBenefitValue:N2}  
- *Ajuste por Taxa de Falha:* R$ {result.SuccessBenefitValue * input.FailureRate:N2}  
- *Benefício de Sucesso Ajustado:* R$ {result.AdjustedSuccessBenefit:N2}  

### Benefícios Totais:
- **Benefícios Totais Ajustados:** R$ {result.TotalBenefits:N2}  

### Resultado Final:
- *Investimento Total:* R$ {input.ProjectBudget:N2}  
- *Retorno sobre Investimento (ROI):* {result.RoiPercentage:F2}%  
- *Viabilidade:* {(result.RoiPercentage > 0 ? "Positiva" : "Negativa")}  
-------------------------
         ";
    }
}