namespace microsoft_hackathon_roi_calculator.Core.Models.Metrics;

/// <summary>
/// Métricas relacionadas a treinamento
/// </summary>
public class TrainingMetrics
{
    private int _completedTraining;
    public int EnrolledTraining { get; set; }
    public int CompletedTraining
    {
        get => _completedTraining; 
        set
        {
            if (value > EnrolledTraining)
                throw new ArgumentException("O treinamento concluído não pode ser maior que o número de inscrição.");

            _completedTraining = value;
        }
    }
    public int PreTrainingScore { get; set; }
    public int PostTrainingScore { get; set; }

    // Employee training completion rate (%) = (Number of employees who completed training ÷ total number of employees enrolled) × 100
    public double EmployeeTrainingCompletionRate { get => (double)CompletedTraining / EnrolledTraining * 100; }

    // Training effectiveness (%) = [(Post-training score − pre-training score) ÷ pre-training score] × 100
    public double TrainingEffectiveness { get => ((double)(PostTrainingScore - PreTrainingScore) / PreTrainingScore) * 100; }
}