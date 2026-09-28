using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Domain.Models;

namespace microsoft_hackathon_roi_calculator.Functions.Functions.v1;

public class ROICalculatorFunction
{
    private readonly ILogger<ROICalculatorFunction> _logger;
    private readonly IROICalculatorService _roiCalculatorService;
    private readonly IAssistantReportService _reportService;

    public ROICalculatorFunction(
        ILogger<ROICalculatorFunction> logger, 
        IROICalculatorService roiCalculatorService,
        IAssistantReportService reportService)
    {
        _logger = logger;
        _roiCalculatorService = roiCalculatorService;
        _reportService = reportService;
    }

    [Function("CalculateROI")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
    {
        req.HttpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
     
        _logger.LogInformation("Processing CalculateROI Azure Function request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var input = JsonSerializer.Deserialize<ROIInputParameters>(requestBody);

        if (input == null)
        {
            return new BadRequestObjectResult("Invalid input: request body could not be deserialized to ROIInputParameters.");
        }

        try
        {
            var result = _roiCalculatorService.CalculateROI(input);
            var mathematicalReport = _roiCalculatorService.GenerateReport(result, input);

            var executiveReport = await _reportService.GenerateInsightsAsync(mathematicalReport);

            var combinedReport = $"{mathematicalReport}\n\n{executiveReport}";
            return new OkObjectResult(combinedReport);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing ROI calculation in Azure Function.");
            return new BadRequestObjectResult($"Calculation error: {ex.Message}");
        }
    }
}
