namespace microsoft_hackathon_roi_calculator.Core.Models.Metrics;

/// <summary>
/// Métricas relacionadas a processos
/// </summary>
public class ProcessMetrics
{
    public int TotalProcesses { get; set; }
    public int CompliantProcesses
    {
        get => CompliantProcesses;
        set
        {
            if (value > TotalProcesses)
                throw new ArgumentException("Compliant processes cannot be greater than total processes");
        }
    }
    // Process compliance rating (%) = (Number of compliant processes ÷ total number of processes) × 100
    public double GetComplianceRate() => (double) CompliantProcesses / TotalProcesses * 100;
}

