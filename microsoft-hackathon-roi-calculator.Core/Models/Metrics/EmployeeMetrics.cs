namespace microsoft_hackathon_roi_calculator.Core.Models.Metrics;

/// <summary>
/// Métricas relacionadas a funcionários
/// </summary>
public class EmployeeMetrics
{
    public int TotalEmployees { get; set; }
    public int TotalHoursWorkedWeekly { get; set; }
    public int EmployeesUsingNewTool
    {
        get => EmployeesUsingNewTool;

        set
        {
            if (value > TotalEmployees)
                throw new ArgumentException("Employees using new tool cannot be greater than total employees");
        }
    }

    // Total EPR = Total hours worked weekly ÷ total number of employees
    public double GetTotalEmployeeProductivityRate() => (double) TotalHoursWorkedWeekly / TotalEmployees;
    // Adoption rate (%) = (Number of employees using the new process/tool ÷ total number of employees) × 100
    public double GetEmployeeAdoptionRate() => (double)EmployeesUsingNewTool / TotalEmployees * 100;
}
