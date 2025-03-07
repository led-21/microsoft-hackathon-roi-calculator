namespace microsoft_hackathon_roi_calculator.Core.Models.Metrics;

/// <summary>
/// Métricas relacionadas a treinamento
/// </summary>
public class TrainingMetrics
{
    public int CompletedTraining { get; set; }
    public int EnrolledTraining { get; set; }
    public int PreTrainingScore { get; set; }
    public int PostTrainingScore { get; set; }
}