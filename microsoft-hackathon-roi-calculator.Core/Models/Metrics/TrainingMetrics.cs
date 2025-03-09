namespace microsoft_hackathon_roi_calculator.Core.Models.Metrics;

/// <summary>
/// Métricas relacionadas a treinamento
/// </summary>
public class TrainingMetrics
{
    public int EnrolledTraining { get; set; }
    public int CompletedTraining
    {
        get => CompletedTraining; set
        {
            if (value > EnrolledTraining)
                throw new ArgumentException("Completed training cannot be greater than enrolled training");
        }
    }
    public int PreTrainingScore { get; set; }
    public int PostTrainingScore { get; set; }

    // Employee training completion rate (%) = (Number of employees who completed training ÷ total number of employees enrolled) × 100
    public double EmployeeTrainingCompletionRate => (double)CompletedTraining / EnrolledTraining * 100;

    // Training effectiveness (%) = [(Post-training score − pre-training score) ÷ pre-training score] × 100
    public double TrainingEffectiveness => ((double)(PostTrainingScore - PreTrainingScore) / PreTrainingScore) * 100;
}