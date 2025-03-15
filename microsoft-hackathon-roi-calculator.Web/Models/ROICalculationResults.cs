namespace microsoft_hackathon_roi_calculator.Web.Models
{
    public class ROICalculationResults
    {
        public double TotalInvestment { get; set; }
        public double TotalBenefits { get; set; }
        public double RoiPercentage { get; set; }
        public double ProductivityGainValue { get; set; }
        public double AdjustedProductivityGainValue { get; set; }
        public double RiskReductionValue { get; set; }
        public double AdjustedRiskReduction { get; set; }
        public double SuccessBenefitValue { get; set; }
        public double AdjustedSuccessBenefit { get; set; }
        public List<string> ActionableRecommendations { get; set; } = new();
    }
}