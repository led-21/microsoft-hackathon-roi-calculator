namespace microsoft_hackathon_roi_calculator.Core.Models.Metrics;

/// <summary>
/// Métricas relacionadas a implementação
/// </summary>
public class ImplementationMetrics
{
    public int TotalChangeImplementationTime { get; set; }
    public int TotalPlannedImplementationTime { get; set; }

    // Speed of change (%) = (Total change implementation time ÷ total planned implementation time​) × 100
    public double SpeedOfChange { get => (double)TotalChangeImplementationTime / TotalPlannedImplementationTime * 100; }
}