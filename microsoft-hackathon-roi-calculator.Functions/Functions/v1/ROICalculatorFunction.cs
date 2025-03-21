using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Domain.Models;
using Azure.AI.OpenAI;
using Azure;

namespace microsoft_hackathon_roi_calculator.Functions.Functions.v1
{
    public class ROICalculatorFunction
    {
        private readonly ILogger<ROICalculatorFunction> _logger;
        private readonly IROICalculatorService _roiCalculatorService;
        private readonly AzureOpenAIClient _openAIClient;

        public ROICalculatorFunction(ILogger<ROICalculatorFunction> logger, IROICalculatorService roiCalculatorService)
        {
            _logger = logger;
            _roiCalculatorService = roiCalculatorService;

            _openAIClient = new AzureOpenAIClient(new Uri("url"), new AzureKeyCredential("openaikey"));
        }

        [Function("CalculateROI")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            req.Headers.AccessControlAllowOrigin = "*";
         
            _logger.LogInformation("C# HTTP trigger to process a Calculate ROI function request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonSerializer.Deserialize<ROIInputParameters>(requestBody);

            if (input == null)
            {
                return new BadRequestObjectResult("Invalid input");
            }

            string openAIReport;

            try
            {
                var result = _roiCalculatorService.CalculateROI(input);
                var report = _roiCalculatorService.GenerateReport(result, input);

                var insights = await GenerateInsightsFromOpenAI(report);

                openAIReport = _roiCalculatorService.GenerateReport(result, input) + "\n" + insights;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error: " + ex.Message);
                openAIReport = "Error: " + ex.Message;
                return new BadRequestObjectResult(openAIReport);
            }

            var response = new OkObjectResult(openAIReport);

            return response;
        }

        private async Task<string> GenerateInsightsFromOpenAI(string report)
        {
            var prompt = $"""
                Com base nos seguintes resultados de cálculo de ROI, forneça insights e recomendações:
                
                Estrutura do Relatório Final

                Resumo Executivo: Visão geral.
                Análise Detalhada: Explicação dos cálculos e dados analisados.
                Insights e Recomendações: Conclusões acionáveis baseadas na análise.

                Relatorio: {report}
                """;
               

            var completionResult = await _openAIClient.GetChatClient("gpt-4o-mini").CompleteChatAsync(prompt).ConfigureAwait(false);

            return completionResult.Value.Content[0].Text;
        }
    }
}