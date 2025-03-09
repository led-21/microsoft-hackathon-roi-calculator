using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microsoft_hackathon_roi_calculator.Core.Models
{
    public class ROICalculatorResult
    {
        public double TotalInvestment { get; set; }
        public double TotalBenefits { get; set; }
        public double RoiPercentage { get; set; }
        public double PotentialFailureCost { get; set; }
        public double DisengagementCost { get; set; }
        public double ProductivitySavings { get; set; }
        public double RiskReductionSavings { get; set; }
        public List<string> ActionableRecommendations { get; set; } = new();
    }
}
