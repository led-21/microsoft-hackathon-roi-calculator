using microsoft_hackathon_roi_calculator.Core.Interfaces;
using microsoft_hackathon_roi_calculator.Core.Models.Metrics;

namespace microsoft_hackathon_roi_calculator.Core.Services;

public class ROICalculatorService : IROICalculatorService
{
    private const decimal PERCENTAGE_MULTIPLIER = 100m;
    private const decimal DEFAULT_RESULT = 0m;

    private decimal CalculatePercentage(decimal numerator, decimal denominator)
    {
        return denominator == 0 ? DEFAULT_RESULT : (numerator / denominator) * PERCENTAGE_MULTIPLIER;
    }

    private void ValidateMetrics<T>(T metrics)
    {
        ArgumentNullException.ThrowIfNull(metrics, nameof(metrics));

        // Validações específicas para cada tipo de métrica
        switch (metrics)
        {
            case CostBenefitMetrics costBenefit:
                // Não aplicamos validação aqui, pois o cálculo de ROI pode aceitar valores negativos ou quaisquer relações
                break;

            case ProcessMetrics process:
                if (process.CompliantProcesses > process.TotalProcesses)
                    throw new ArgumentException("O número de processos conformes não pode ser maior que o total de processos.", nameof(process));
                break;

            case TrainingMetrics training:
                if (training.CompletedTraining > training.EnrolledTraining)
                    throw new ArgumentException("O número de treinamentos concluídos não pode ser maior que o número de treinamentos inscritos.", nameof(training));
                break;

            case EmployeeMetrics employee:
                if (employee.EmployeesUsingNewTool > employee.TotalEmployees)
                    throw new ArgumentException("O número de funcionários usando a nova ferramenta não pode ser maior que o total de funcionários.", nameof(employee));
                break;

            case ResponseMetrics response:
                if (response.PositiveResponses > response.TotalResponses)
                    throw new ArgumentException("O número de respostas positivas não pode ser maior que o total de respostas.", nameof(response));
                break;

            case ImplementationMetrics implementation:
                if (implementation.TotalChangeImplementationTime > implementation.TotalPlannedImplementationTime)
                    throw new ArgumentException("O tempo total de implementação de mudança não pode ser maior que o tempo planejado.", nameof(implementation));
                break;
        }
    }

    public decimal CalculateROI(CostBenefitMetrics metrics)
    {
        ValidateMetrics(metrics);
        return CalculatePercentage(metrics.NetBenefit - metrics.CostOfInvestment, metrics.CostOfInvestment);
    }

    public decimal CalculateProcessComplianceRate(ProcessMetrics metrics)
    {
        ValidateMetrics(metrics);
        return CalculatePercentage(metrics.CompliantProcesses, metrics.TotalProcesses);
    }

    public decimal CalculateTrainingCompletionRate(TrainingMetrics metrics)
    {
        ValidateMetrics(metrics);
        return CalculatePercentage(metrics.CompletedTraining, metrics.EnrolledTraining);
    }

    public decimal CalculateEmployeeAdoptionRate(EmployeeMetrics metrics)
    {
        ValidateMetrics(metrics);
        return CalculatePercentage(metrics.EmployeesUsingNewTool, metrics.TotalEmployees);
    }

    public decimal CalculateAverageScore(ResponseMetrics metrics)
    {
        ValidateMetrics(metrics);
        return metrics.TotalResponses == 0 ? DEFAULT_RESULT : (decimal)metrics.SumOfAllScores / metrics.TotalResponses;
    }

    public decimal CalculatePositiveResponseRate(ResponseMetrics metrics)
    {
        ValidateMetrics(metrics);
        return CalculatePercentage(metrics.PositiveResponses, metrics.TotalResponses);
    }

    public int CalculateTrainingScoreImprovement(TrainingMetrics metrics)
    {
        ValidateMetrics(metrics);
        return metrics.PostTrainingScore - metrics.PreTrainingScore;
    }

    public decimal CalculateImplementationEfficiency(ImplementationMetrics metrics)
    {
        ValidateMetrics(metrics);
        return CalculatePercentage(metrics.TotalChangeImplementationTime, metrics.TotalPlannedImplementationTime);
    }
}