using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using microsoft_hackathon_roi_calculator.Application.Configuration;
using microsoft_hackathon_roi_calculator.Application.Interfaces;

namespace microsoft_hackathon_roi_calculator.Application.Services;

/// <summary>
/// Hybrid report service that connects to Azure OpenAI when configured,
/// and seamlessly falls back to LocalTemplateReportService when cloud services are offline or unconfigured.
/// </summary>
public class AzureOpenAIReportService : IAssistantReportService
{
    private readonly AzureOpenAIOptions _options;
    private readonly ILogger<AzureOpenAIReportService> _logger;
    private readonly LocalTemplateReportService _fallbackService;
    private readonly AzureOpenAIClient? _openAIClient;

    public AzureOpenAIReportService(
        IOptions<AzureOpenAIOptions> options,
        ILogger<AzureOpenAIReportService> logger,
        LocalTemplateReportService fallbackService)
    {
        _options = options.Value;
        _logger = logger;
        _fallbackService = fallbackService;

        if (_options.IsConfigured)
        {
            try
            {
                _openAIClient = new AzureOpenAIClient(new Uri(_options.Endpoint!), new AzureKeyCredential(_options.ApiKey!));
                _logger.LogInformation("Azure OpenAI client initialized successfully with deployment {Deployment}.", _options.DeploymentName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to initialize Azure OpenAI client. Offline fallback will be used.");
                _openAIClient = null;
            }
        }
        else
        {
            _logger.LogInformation("Azure OpenAI is not configured. Running in offline mode with local template engine.");
        }
    }

    public async Task<string> GenerateInsightsAsync(string reportMarkdown, CancellationToken cancellationToken = default)
    {
        if (_openAIClient == null || !_options.IsConfigured)
        {
            return await _fallbackService.GenerateInsightsAsync(reportMarkdown, cancellationToken);
        }

        try
        {
            var prompt = $"""
                Com base nos seguintes resultados de cálculo de ROI, forneça insights e recomendações executivas para líderes de transformação:

                Estrutura do Relatório Final:
                1. Resumo Executivo: Visão geral e prontidão organizacional.
                2. Análise Detalhada: Diagnóstico dos cálculos e premissas de risco.
                3. Insights e Recomendações: Ações prioritárias para garantir a captura de valor.

                Relatório Base:
                {reportMarkdown}
                """;

            var chatClient = _openAIClient.GetChatClient(_options.DeploymentName);
            OpenAI.Chat.ChatMessage[] messages = [new OpenAI.Chat.UserChatMessage(prompt)];
            var response = await chatClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);

            if (response.Value?.Content?.Count > 0)
            {
                return response.Value.Content[0].Text;
            }

            _logger.LogWarning("Azure OpenAI returned empty content. Falling back to local report engine.");
            return await _fallbackService.GenerateInsightsAsync(reportMarkdown, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Azure OpenAI service call failed. Gracefully falling back to local deterministic report engine.");
            return await _fallbackService.GenerateInsightsAsync(reportMarkdown, cancellationToken);
        }
    }
}
