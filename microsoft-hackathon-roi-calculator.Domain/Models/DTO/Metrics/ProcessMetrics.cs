using System.ComponentModel.DataAnnotations;

namespace microsoft_hackathon_roi_calculator.Domain.Models.DTO.Metrics;

/// <summary>
/// Métricas relacionadas a processos
/// </summary>
public class ProcessMetrics
{
    private int _compliantProcesses;
    public int TotalProcesses { get; set; }
    public int CompliantProcesses
    {
        get => _compliantProcesses;
        set
        {
            if (value > TotalProcesses)
                throw new ArgumentException("Os processos em conformidade não podem ser maiores que os processos totais.");

            _compliantProcesses = value;
        }
    }
    // Process compliance rating (%) = (Number of compliant processes ÷ total number of processes) × 100
    public double ComplianceRate { get => (double)CompliantProcesses / TotalProcesses * 100; }
}