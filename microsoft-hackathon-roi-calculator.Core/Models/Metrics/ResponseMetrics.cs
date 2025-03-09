namespace microsoft_hackathon_roi_calculator.Core.Models.Metrics;

/// <summary>
/// Métricas relacionadas a respostas e scores
/// </summary>
public class ResponseMetrics
{
    public int SumOfAllScores { get; set; }
    public int TotalResponses { get; set; }
    public int PositiveResponses
    {
        get => PositiveResponses;
        set
        {
            if (value > TotalResponses)
                throw new ArgumentException("Positive responses cannot be greater than total responses");
        }
    }

    // CSAT score (%) = (Sum of all scores ÷ total number of responses) × 100
    public double GetCSATScore() => (double)SumOfAllScores / TotalResponses * 100;

    // ESI (%) = (Total number of employees who gave positive survey responses ÷ total number of employees who took the survey) × 100
    public double GetEmployeeSatisfactionIndex() => (double)PositiveResponses / TotalResponses * 100;
}

