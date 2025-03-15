using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Domain.Models;

namespace microsoft_hackathon_roi_calculator.Functions.Functions.v1
{
    public class EstimateFailureRate
    {
        private readonly ILogger<EstimateFailureRate> _logger;

        private readonly IROICalculatorService _roiCalculatorService;

        public EstimateFailureRate(ILogger<EstimateFailureRate> logger, IROICalculatorService roiCalculatorService)
        {
            _logger = logger;
            _roiCalculatorService = roiCalculatorService;
        }

        [Function("EstimateFailureRate")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonSerializer.Deserialize<ROIInputParameters>(requestBody);

            if (input == null)
            {
                return new BadRequestObjectResult("Please pass a valid input.");
            }

            var estimate = _roiCalculatorService.EstimateFailureRate(input);

            return new OkObjectResult(new { EstimateFailureRate = estimate });
        }
    }
}