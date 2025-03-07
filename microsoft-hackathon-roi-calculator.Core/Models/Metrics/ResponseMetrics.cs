namespace microsoft_hackathon_roi_calculator.Core.Models.Metrics;

/// <summary>
/// Métricas relacionadas a respostas e scores
/// </summary>
public class ResponseMetrics
{
    public int SumOfAllScores { get; set; }
    public int TotalResponses { get; set; }
    public int PositiveResponses { get; set; }
}

