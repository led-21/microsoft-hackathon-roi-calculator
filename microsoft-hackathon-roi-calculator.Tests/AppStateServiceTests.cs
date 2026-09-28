using microsoft_hackathon_roi_calculator.Domain.Models;
using microsoft_hackathon_roi_calculator.Web.Services;
using Xunit;

namespace microsoft_hackathon_roi_calculator.Tests;

/// <summary>
/// Unit tests for <see cref="AppStateService"/>, verifying initial state defaults,
/// calculation result management, project CRUD operations, auto-ID assignment,
/// and OnChange event notification behavior.
/// </summary>
public class AppStateServiceTests
{
    // ───────────────────────── Initial State ─────────────────────────

    [Fact]
    public void CurrentInput_InitialState_HasExpectedDefaults()
    {
        // Arrange & Act
        var sut = new AppStateService();

        // Assert
        Assert.Equal(1_000_000, sut.CurrentInput.ProjectBudget);
        Assert.Equal(250, sut.CurrentInput.NumberOfEmployees);
        Assert.Equal(12, sut.CurrentInput.ProjectDurationMonths);
        Assert.Equal(0.55, sut.CurrentInput.FailureRate);
        Assert.Equal(0.35, sut.CurrentInput.BudgetLossRate);
        Assert.Equal(0.15, sut.CurrentInput.ExpectedDisengagementRate);
        Assert.Equal(1.20, sut.CurrentInput.ExpectedProductivityGain);
        Assert.Equal(0.40, sut.CurrentInput.ProjectedRiskReduction);
        Assert.Equal(0.30, sut.CurrentInput.ExpectedSuccessBenefit);
    }

    [Fact]
    public void Projects_InitialState_ContainsThreeSeedProjects()
    {
        // Arrange & Act
        var sut = new AppStateService();

        // Assert
        Assert.Equal(3, sut.Projects.Count);
    }

    // ───────────────────── HasCalculated ─────────────────────

    [Fact]
    public void HasCalculated_WhenNoCalculationDone_ReturnsFalse()
    {
        // Arrange
        var sut = new AppStateService();

        // Act & Assert
        Assert.False(sut.HasCalculated);
    }

    // ───────────────── SetCalculationResult ──────────────────

    [Fact]
    public void SetCalculationResult_SetsMarkdownAndHtml_AndHasCalculatedBecomesTrue()
    {
        // Arrange
        var sut = new AppStateService();

        // Act
        sut.SetCalculationResult("# ROI Report", "<h1>ROI Report</h1>");

        // Assert
        Assert.Equal("# ROI Report", sut.CalculationResultMarkdown);
        Assert.Equal("<h1>ROI Report</h1>", sut.CalculationResultHtml);
        Assert.True(sut.HasCalculated);
    }

    [Fact]
    public void SetCalculationResult_FiresOnChangeEvent()
    {
        // Arrange
        var sut = new AppStateService();
        var fired = false;
        sut.OnChange += () => fired = true;

        // Act
        sut.SetCalculationResult("md", "html");

        // Assert
        Assert.True(fired);
    }

    // ───────────────────── SetDetailReport ─────────────────────

    [Fact]
    public void SetDetailReport_SetsDetailReportProperty()
    {
        // Arrange
        var sut = new AppStateService();

        // Act
        sut.SetDetailReport("Detailed analysis content");

        // Assert
        Assert.Equal("Detailed analysis content", sut.DetailReport);
    }

    [Fact]
    public void SetDetailReport_FiresOnChangeEvent()
    {
        // Arrange
        var sut = new AppStateService();
        var fired = false;
        sut.OnChange += () => fired = true;

        // Act
        sut.SetDetailReport("report");

        // Assert
        Assert.True(fired);
    }

    // ───────────────────── SetProjects ─────────────────────

    [Fact]
    public void SetProjects_ReplacesProjectListEntirely()
    {
        // Arrange
        var sut = new AppStateService();
        var newProjects = new List<ProjectROI>
        {
            new() { Id = 10, ProjectName = "Alpha" },
            new() { Id = 20, ProjectName = "Beta" }
        };

        // Act
        sut.SetProjects(newProjects);

        // Assert
        Assert.Equal(2, sut.Projects.Count);
        Assert.Equal("Alpha", sut.Projects[0].ProjectName);
        Assert.Equal("Beta", sut.Projects[1].ProjectName);
    }

    // ───────────────────── AddProject ─────────────────────

    [Fact]
    public void AddProject_AddsProjectToList()
    {
        // Arrange
        var sut = new AppStateService();
        var initialCount = sut.Projects.Count;
        var project = new ProjectROI { Id = 99, ProjectName = "New Project" };

        // Act
        sut.AddProject(project);

        // Assert
        Assert.Equal(initialCount + 1, sut.Projects.Count);
        Assert.Contains(sut.Projects, p => p.Id == 99 && p.ProjectName == "New Project");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void AddProject_WhenIdIsZeroOrNegative_AutoAssignsNextId(int invalidId)
    {
        // Arrange
        var sut = new AppStateService();
        var expectedId = sut.Projects.Max(p => p.Id) + 1;
        var project = new ProjectROI { Id = invalidId, ProjectName = "Auto-ID Project" };

        // Act
        sut.AddProject(project);

        // Assert
        Assert.Equal(expectedId, project.Id);
        Assert.Contains(sut.Projects, p => p.Id == expectedId);
    }

    [Fact]
    public void AddProject_WhenListIsEmpty_AutoAssignsIdOne()
    {
        // Arrange
        var sut = new AppStateService();
        sut.SetProjects(Enumerable.Empty<ProjectROI>());
        var project = new ProjectROI { Id = 0, ProjectName = "First" };

        // Act
        sut.AddProject(project);

        // Assert
        Assert.Equal(1, project.Id);
    }

    [Fact]
    public void AddProject_FiresOnChangeEvent()
    {
        // Arrange
        var sut = new AppStateService();
        var fired = false;
        sut.OnChange += () => fired = true;

        // Act
        sut.AddProject(new ProjectROI { Id = 50, ProjectName = "Test" });

        // Assert
        Assert.True(fired);
    }

    // ───────────────────── UpdateProject ─────────────────────

    [Fact]
    public void UpdateProject_ReplacesExistingProjectById()
    {
        // Arrange
        var sut = new AppStateService();
        var updated = new ProjectROI { Id = 1, ProjectName = "Updated Name", ProjectBudget = 999 };

        // Act
        sut.UpdateProject(updated);

        // Assert
        var project = sut.Projects.First(p => p.Id == 1);
        Assert.Equal("Updated Name", project.ProjectName);
        Assert.Equal(999, project.ProjectBudget);
    }

    [Fact]
    public void UpdateProject_WhenIdNotFound_DoesNotThrow()
    {
        // Arrange
        var sut = new AppStateService();
        var nonExistent = new ProjectROI { Id = 9999, ProjectName = "Ghost" };

        // Act & Assert — should not throw
        var exception = Record.Exception(() => sut.UpdateProject(nonExistent));
        Assert.Null(exception);
    }

    [Fact]
    public void UpdateProject_WhenIdNotFound_DoesNotFireOnChange()
    {
        // Arrange
        var sut = new AppStateService();
        var fired = false;
        sut.OnChange += () => fired = true;

        // Act
        sut.UpdateProject(new ProjectROI { Id = 9999, ProjectName = "Ghost" });

        // Assert — OnChange only fires inside the if-block when index >= 0
        Assert.False(fired);
    }

    // ───────────────────── RemoveProject ─────────────────────

    [Fact]
    public void RemoveProject_RemovesProjectById()
    {
        // Arrange
        var sut = new AppStateService();
        Assert.Contains(sut.Projects, p => p.Id == 2);

        // Act
        sut.RemoveProject(2);

        // Assert
        Assert.DoesNotContain(sut.Projects, p => p.Id == 2);
    }

    [Fact]
    public void RemoveProject_WhenIdNotFound_DoesNotThrow()
    {
        // Arrange
        var sut = new AppStateService();
        var initialCount = sut.Projects.Count;

        // Act & Assert — should not throw
        var exception = Record.Exception(() => sut.RemoveProject(9999));
        Assert.Null(exception);
        Assert.Equal(initialCount, sut.Projects.Count);
    }

    [Fact]
    public void RemoveProject_FiresOnChangeEvent()
    {
        // Arrange
        var sut = new AppStateService();
        var fired = false;
        sut.OnChange += () => fired = true;

        // Act
        sut.RemoveProject(1);

        // Assert
        Assert.True(fired);
    }

    // ───────────────── Multiple OnChange Subscribers ─────────────────

    [Fact]
    public void OnChange_MultipleSubscribers_AllAreNotified()
    {
        // Arrange
        var sut = new AppStateService();
        var subscriber1Notified = false;
        var subscriber2Notified = false;
        var subscriber3Notified = false;

        sut.OnChange += () => subscriber1Notified = true;
        sut.OnChange += () => subscriber2Notified = true;
        sut.OnChange += () => subscriber3Notified = true;

        // Act
        sut.SetCalculationResult("md", "html");

        // Assert
        Assert.True(subscriber1Notified);
        Assert.True(subscriber2Notified);
        Assert.True(subscriber3Notified);
    }
}
