
namespace microsoft_hackathon_roi_calculator.Core.Models
{
    public class ROICalculationMetrics 
    {
        public int CompliantProcesses { get; set; }
        public int TotalProcesses { get; set; }
        public int TotalHoursWorked { get; set; }
        public int TotalEmployees { get; set; }
        public decimal NetBenefit { get; set; }
        public decimal CostOfInvestment { get; set; }
        public int CompletedTraining { get; set; }
        public int EnrolledTraining { get; set; }
        public int EmployeesUsingNewTool { get; set; }
        public int SumOfAllScores { get; set; }
        public int TotalResponses { get; set; }
        public int PositiveResponses { get; set; }
        public int PreTrainingScore { get; set; }
        public int PostTrainingScore { get; set; }
        public int TotalChangeImplementationTime { get; set; }
        public int TotalPlannedImplementationTime { get; set; }
    }
}
