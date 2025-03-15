using System.ComponentModel.DataAnnotations;


namespace microsoft_hackathon_roi_calculator.Domain.Models
{
    public class ProjectROI
    {
        [Key]
        public int Id { get; set; }
        public required string ProjectName { get; set; } // Nome do projeto
        public double ProjectBudget { get; set; } // Orçamento do projeto em reais
        public int NumberOfEmployees { get; set; } // Número de funcionários impactados
        public int ProjectDurationMonths { get; set; } // Duração do projeto em meses
        public double ROI { get; set; } // Retorno sobre o investimento
    }
}