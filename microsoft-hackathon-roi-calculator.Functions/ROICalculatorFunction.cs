using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using microsoft_hackathon_roi_calculator.Core.Models;

using microsoft_hackathon_roi_calculator.Core.Interfaces;
using System.Text.Json;

namespace microsoft_hackathon_roi_calculator.Functions
{
    public class ROICalculatorFunction
    {
        private readonly ILogger<ROICalculatorFunction> _logger;
        private readonly IROICalculatorService _roiCalculatorService;

        public ROICalculatorFunction(ILogger<ROICalculatorFunction> logger, IROICalculatorService roiCalculatorService)
        {
            _logger = logger;
            _roiCalculatorService = roiCalculatorService;
        }

        [Function("CalculateROI")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger Calculate ROI function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonSerializer.Deserialize<ROIInputParameters>(requestBody);

            if (input == null)
            {
                return new BadRequestObjectResult("Invalid input");
            }

            string report = null;

            try
            {
                var result = _roiCalculatorService.CalculateROI(input);
                report = _roiCalculatorService.GenerateReport(result, input);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error: "+ex.Message);
            }

            return new OkObjectResult(report??"Algum erro ocorreu");
        }
    }
}
