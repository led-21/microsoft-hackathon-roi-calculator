using System.ComponentModel.DataAnnotations;

namespace microsoft_hackathon_roi_calculator.Domain.Models.DTO.Metrics;

/// <summary>
/// Métricas relacionadas a respostas e scores
/// </summary>
public class ResponseMetrics
{
    private int _positiveResponses;
    public int SumOfAllScores { get; set; }
    public int TotalResponses { get; set; }
    public int PositiveResponses
    {
        get => _positiveResponses;
        set
        {
            if (value > TotalResponses)
                throw new ArgumentException("As respostas positivas não podem ser maiores que o total de respostas.");

            _positiveResponses = value;
        }
    }

    // CSAT score (%) = (Sum of all scores ÷ total number of responses) × 100
    public double GetCSATScore() => (double)SumOfAllScores / TotalResponses * 100;

    // ESI (%) = (Total number of employees who gave positive survey responses ÷ total number of employees who took the survey) × 100
    public double EmployeeSatisfactionIndex { get => (double)PositiveResponses / TotalResponses * 100; }
}