namespace microsoft_hackathon_roi_calculator.Application.Interfaces;

/// <summary>
/// Service contract for generating AI narrative analysis and strategic recommendations.
/// </summary>
public interface IAssistantReportService
{
    Task<string> GenerateInsightsAsync(string reportMarkdown, CancellationToken cancellationToken = default);
}
