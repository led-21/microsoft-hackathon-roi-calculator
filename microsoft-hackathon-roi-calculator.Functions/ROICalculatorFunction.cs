using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace microsoft_hackathon_roi_calculator.Functions
{
    public class ROICalculatorFunction
    {
        private readonly ILogger<ROICalculatorFunction> _logger;

        public ROICalculatorFunction(ILogger<ROICalculatorFunction> logger)
        {
            _logger = logger;
        }

        [Function("CalculateROI")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
