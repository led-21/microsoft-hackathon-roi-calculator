using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsoft_hackathon_roi_calculator.Core.Models
{
    public class ProjectInput
    {
        public double ProjectBudget { get; set; } // Orçamento do projeto em reais
        public int NumberOfEmployees { get; set; } // Número de funcionários impactados
        public int ProjectDurationMonths { get; set; } // Duração do projeto em meses
        public double BudgetLossRate { get; set; } = 0.5; // Percentual do orçamento que seria perdido caso o projeto falhe
        public double FailureRate { get; set; } = 0.7; // Taxa de falha do projeto
        public double ExpectedDisengagementRate { get; set; } = 0.15; // Taxa de desengajamento
        public double ExpectedProductivityGain { get; set; } = 1.5; // Ganho de produtividade esperado
        public double ProjectedRiskReduction { get; set; } = 0.7; // Redução de risco projetada
    }
}
