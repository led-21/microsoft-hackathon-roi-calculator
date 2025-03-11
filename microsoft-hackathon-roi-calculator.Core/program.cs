using microsoft_hackathon_roi_calculator.Core.Models;
using microsoft_hackathon_roi_calculator.Core.Models.Metrics;
using microsoft_hackathon_roi_calculator.Core.Services;


var calculator = new ROICalculatorService();

var input = new ROIInputParameters
{
    ProjectBudget = 1000000, // R$ 1 milhão
    NumberOfEmployees = 50,
    ProjectDurationMonths = 12
    // Taxas padrão já definidas na classe ProjectInput
};

try
{
    var result = calculator.CalculateROI(input);
    var report = calculator.GenerateReport(result, input);
    Console.WriteLine(report);
}
catch (Exception ex)
{
    Console.WriteLine($"Erro: {ex.Message}");
}

try
{
    EmployeeMetrics employeeMetrics = new() { TotalEmployees = 100, TotalHoursWorkedWeekly = 500, EmployeesUsingNewTool = 120 };

    Console.WriteLine(employeeMetrics.EmployeeAdoptionRate);
}
catch (Exception ex)
{
    Console.WriteLine($"Erro: {ex.Message}");
}