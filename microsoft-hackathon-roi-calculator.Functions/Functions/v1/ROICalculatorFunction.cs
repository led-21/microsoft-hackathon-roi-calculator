using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Domain.Models;
using Azure.AI.OpenAI;
using Azure;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net;
using OpenAI.Chat;

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
            _openAIClient = new AzureOpenAIClient(new Uri("url"),
                new AzureKeyCredential("apiKey"));
        }

        [Function("CalculateROI")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger to process a Calculate ROI function request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonSerializer.Deserialize<ROIInputParameters>(requestBody);

            if (input == null)
            {
                return new BadRequestObjectResult("Invalid input");
            }

            string report = string.Empty;

            try
            {
                var result = _roiCalculatorService.CalculateROI(input);

                var insights = await GenerateInsightsFromOpenAI(result);

                report = _roiCalculatorService.GenerateReport(result, input) + "\n" + insights;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error: " + ex.Message);
                report = "Error: " + ex.Message;
                return new BadRequestObjectResult(report);
            }

            return new OkObjectResult(report);
        }

        private async Task<string> GenerateInsightsFromOpenAI(ROICalculationResults result)
        {
            var prompt = $"Com base nos seguintes resultados de cálculo de ROI, forneça insights e recomendações:\n" +
                         $"Investimento Total: {result.TotalInvestment}\n" +
                         $"Benefícios Totais: {result.TotalBenefits}\n" +
                         $"Percentual de ROI: {result.RoiPercentage}\n" +
                         $"Valor do Ganho de Produtividade: {result.ProductivityGainValue}\n" +
                         $"Valor Ajustado do Ganho de Produtividade: {result.AdjustedProductivityGainValue}\n" +
                         $"Valor da Redução de Risco: {result.RiskReductionValue}\n" +
                         $"Redução de Risco Ajustada: {result.AdjustedRiskReduction}\n" +
                         $"Valor do Benefício de Sucesso: {result.SuccessBenefitValue}\n" +
                         $"Benefício de Sucesso Ajustado: {result.AdjustedSuccessBenefit}\n" +
                         $"Recomendações Ação: {string.Join(", ", result.ActionableRecommendations)}";


            var completionResult = await _openAIClient.GetChatClient("gpt-4o-mini").CompleteChatAsync(prompt).ConfigureAwait(false);
            return completionResult.Value.Content[0].Text;
        }
    }
}