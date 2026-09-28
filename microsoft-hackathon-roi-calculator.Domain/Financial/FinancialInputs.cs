namespace microsoft_hackathon_roi_calculator.Domain.Financial;

/// <summary>
/// Core project inputs for financial modeling.
/// </summary>
/// <param name="ProjectBudget">Total allocated project budget in currency units (R$). Must be greater than zero.</param>
/// <param name="NumberOfEmployees">Total headcount impacted by the transformation. Must be greater than zero.</param>
/// <param name="ProjectDurationMonths">Estimated execution duration in months. Must be greater than zero.</param>
public record FinancialInputs
{
    public double ProjectBudget { get; init; }
    public int NumberOfEmployees { get; init; }
    public int ProjectDurationMonths { get; init; }

    public FinancialInputs(double projectBudget, int numberOfEmployees, int projectDurationMonths)
    {
        if (projectBudget <= 0)
            throw new ArgumentOutOfRangeException(nameof(projectBudget), "Project budget must be greater than zero.");
        if (numberOfEmployees <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfEmployees), "Number of employees must be greater than zero.");
        if (projectDurationMonths <= 0)
            throw new ArgumentOutOfRangeException(nameof(projectDurationMonths), "Project duration must be greater than zero.");

        ProjectBudget = projectBudget;
        NumberOfEmployees = numberOfEmployees;
        ProjectDurationMonths = projectDurationMonths;
    }
}
