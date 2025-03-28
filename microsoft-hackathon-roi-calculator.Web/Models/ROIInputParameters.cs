﻿namespace microsoft_hackathon_roi_calculator.Web.Models
{
    public class ROIInputParameters
    {
        public double ProjectBudget { get; set; } // Orçamento do projeto em reais
        public int NumberOfEmployees { get; set; } // Número de funcionários impactados
        public int ProjectDurationMonths { get; set; } // Duração do projeto em meses

        // Parâmetros de Cálculo
        public double BudgetLossRate { get; set; } = 0.3; // Percentual do orçamento perdido em caso de falha (ex.: 0.3 para 30%).
        public double FailureRate { get; set; } = 0.7; // Probabilidade de falha do projeto (ex.: 0.7 para 70%).
        public double ExpectedDisengagementRate { get; set; } = 0.15; // Percentual de perda por desengajamento (ex.: 0.15 para 15%).
        public double ExpectedProductivityGain { get; set; } = 1.5; // Fator de aumento de produtividade (ex.: 1.5 para 50%).
        public double ProjectedRiskReduction { get; set; } = 0; // Percentual de risco mitigado (ex.: 0.5 para 50% ou 0 para 0%).
        public double ExpectedSuccessBenefit { get; set; } = 2; // Multiplicador de benefícios adicionais (ex.: 2 para 200%).
    }
}