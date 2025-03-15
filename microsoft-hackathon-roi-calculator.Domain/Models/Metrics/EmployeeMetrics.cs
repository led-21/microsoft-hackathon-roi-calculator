using System.ComponentModel.DataAnnotations;

namespace microsoft_hackathon_roi_calculator.Domain.Models.Metrics;

/// <summary>
/// Métricas relacionadas a funcionários
/// </summary>
public class EmployeeMetrics
{
    private int _employeesUsingNewTool;
    public int TotalEmployees { get; set; }
    public int TotalHoursWorkedWeekly { get; set; }
    public int EmployeesUsingNewTool
    {
        get => _employeesUsingNewTool;

        set
        {
            if (value > TotalEmployees)
                throw new ArgumentException("O número de funcionários que usam a nova ferramenta não pode ser maior que o total de funcionários.");

            _employeesUsingNewTool = value;
        }
    }

    // Total EPR = Total hours worked weekly ÷ total number of employees
    public double TotalEmployeeProductivityRate { get => (double)TotalHoursWorkedWeekly / TotalEmployees; }

    // Adoption rate (%) = (Number of employees using the new process/tool ÷ total number of employees) × 100
    public double EmployeeAdoptionRate { get => (double)EmployeesUsingNewTool / TotalEmployees * 100; }
}