using System.ComponentModel.DataAnnotations;


namespace microsoft_hackathon_roi_calculator.Web.Models
{
    public class ProjectROI
    {
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty; // Nome do projeto
        public string? Description { get; set; } // Descrição do projeto
        public double ProjectBudget { get; set; } // Orçamento do projeto em reais
        public int NumberOfEmployees { get; set; } // Número de funcionários impactados
        public DateTime StartDate { get; set; } // Data de início do projeto
        public int ProjectDurationMonths { get; set; } // Duração do projeto em meses
        public double ROI { get; set; } // Retorno sobre o investimento

        // Parametros para calcular metricas adicionais
        public int? TotalHoursWorkedWeekly { get; set; }
        public int? CompletedTraining { get; set; }
        public int? EmployeesUsingNewTool { get; set; }
        public int? TotalChangeImplementationTime { get; set; }
        public int? TotalPlannedImplementationTime { get; set; }
        public int? TotalProcesses { get; set; }
        public int? CompliantProcesses { get; set; }
        public int? ProjectEvaluationTotalResponses { get; set; }
        public int? ProjectEvaluationPositiveResponses { get; set; }
        public int? ProjectEvaluationSumOfAllScores { get; set; }
    }
}