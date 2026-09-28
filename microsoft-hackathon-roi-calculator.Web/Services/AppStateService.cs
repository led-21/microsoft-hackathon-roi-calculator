using microsoft_hackathon_roi_calculator.Domain.Models;

namespace microsoft_hackathon_roi_calculator.Web.Services;

/// <summary>
/// Scoped application state service managing shared UI state across Blazor components,
/// eliminating reliance on unsafe static variables and providing offline mock data.
/// </summary>
public class AppStateService
{
    public event Action? OnChange;

    public ROIInputParameters CurrentInput { get; set; } = new()
    {
        ProjectBudget = 1000000,
        NumberOfEmployees = 250,
        ProjectDurationMonths = 12,
        FailureRate = 0.55,
        BudgetLossRate = 0.35,
        ExpectedDisengagementRate = 0.15,
        ExpectedProductivityGain = 1.20,
        ProjectedRiskReduction = 0.40,
        ExpectedSuccessBenefit = 0.30
    };

    public string CalculationResultHtml { get; private set; } = string.Empty;
    public string CalculationResultMarkdown { get; private set; } = string.Empty;
    public string DetailReport { get; private set; } = string.Empty;
    public bool HasCalculated => !string.IsNullOrWhiteSpace(CalculationResultMarkdown);

    public List<ProjectROI> Projects { get; private set; } = GetInitialProjects();

    public void SetCalculationResult(string markdown, string html)
    {
        CalculationResultMarkdown = markdown;
        CalculationResultHtml = html;
        NotifyStateChanged();
    }

    public void SetDetailReport(string report)
    {
        DetailReport = report;
        NotifyStateChanged();
    }

    public void SetProjects(IEnumerable<ProjectROI> projects)
    {
        Projects = projects.ToList();
        NotifyStateChanged();
    }

    public void AddProject(ProjectROI project)
    {
        if (project.Id <= 0)
        {
            project.Id = Projects.Any() ? Projects.Max(p => p.Id) + 1 : 1;
        }
        Projects.Add(project);
        NotifyStateChanged();
    }

    public void UpdateProject(ProjectROI project)
    {
        var index = Projects.FindIndex(p => p.Id == project.Id);
        if (index >= 0)
        {
            Projects[index] = project;
            NotifyStateChanged();
        }
    }

    public void RemoveProject(int id)
    {
        Projects.RemoveAll(p => p.Id == id);
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    private static List<ProjectROI> GetInitialProjects() => new()
    {
        new ProjectROI
        {
            Id = 1,
            ProjectName = "Migração Cloud ERP & Data Analytics",
            ProjectBudget = 1250000,
            NumberOfEmployees = 450,
            ROI = 1.84,
            StartDate = DateTime.UtcNow.AddMonths(-6)
        },
        new ProjectROI
        {
            Id = 2,
            ProjectName = "Implementação CRM Global com IA",
            ProjectBudget = 850000,
            NumberOfEmployees = 280,
            ROI = 2.12,
            StartDate = DateTime.UtcNow.AddMonths(-4)
        },
        new ProjectROI
        {
            Id = 3,
            ProjectName = "Automação Operacional e RPA",
            ProjectBudget = 420000,
            NumberOfEmployees = 120,
            ROI = 1.45,
            StartDate = DateTime.UtcNow.AddMonths(-2)
        }
    };
}
