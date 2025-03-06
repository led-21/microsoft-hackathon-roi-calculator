
namespace microsoft_hackathon_roi_calculator.Core.Models
{
    public class ROICalculationMetrics
    {
        /// <summary>
        /// Métricas relacionadas a processos
        /// </summary>
        public int CompliantProcesses { get; set; }
        public int TotalProcesses { get; set; }

        /// <summary>
        /// Métricas relacionadas a custos e benefícios
        /// </summary>
        public decimal NetBenefit { get; set; }
        public decimal CostOfInvestment { get; set; }

        /// <summary>
        /// Métricas relacionadas a treinamento
        /// </summary>
        public int CompletedTraining { get; set; }
        public int EnrolledTraining { get; set; }
        public int PreTrainingScore { get; set; }
        public int PostTrainingScore { get; set; }

        /// <summary>
        /// Métricas relacionadas a funcionários
        /// </summary>
        public int TotalEmployees { get; set; }
        public int TotalHoursWorked { get; set; }
        public int EmployeesUsingNewTool { get; set; }

        /// <summary>
        /// Métricas relacionadas a respostas e scores
        /// </summary>
        public int SumOfAllScores { get; set; }
        public int TotalResponses { get; set; }
        public int PositiveResponses { get; set; }

        /// <summary>
        /// Métricas relacionadas a implementação
        /// </summary>
        public int TotalChangeImplementationTime { get; set; }
        public int TotalPlannedImplementationTime { get; set; }
    }

}
