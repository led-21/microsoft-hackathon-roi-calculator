using microsoft_hackathon_roi_calculator.Core.Models.Metrics;
using System.ComponentModel.DataAnnotations;


namespace microsoft_hackathon_roi_calculator.Core.Models
{
    public class ProjectROI
    {
        [Key]
        public int Id { get; set; }
        public required string ProjectName { get; set; } // Nome do projeto
        public double ProjectBudget { get; set; } // Orçamento do projeto em reais
        public int NumberOfEmployees { get; set; } // Número de funcionários impactados
        public int ProjectDurationMonths { get; set; } // Duração do projeto em meses

        //Metricas 
        public CostBenefitMetrics? CostBenefitMetrics { get; set; }
        public EmployeeMetrics? EmployeeMetrics { get; set; }
        public TrainingMetrics? TrainingMetrics { get; set; }
        public ImplementationMetrics? ImplementationMetrics { get; set; }
        public ProcessMetrics? ProcessMetrics { get; set; }
        public ResponseMetrics? ResponseMetrics { get; set; }
    }
}
