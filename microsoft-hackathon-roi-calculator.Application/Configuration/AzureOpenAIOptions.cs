namespace microsoft_hackathon_roi_calculator.Application.Configuration;

public class AzureOpenAIOptions
{
    public const string SectionName = "AzureOpenAI";

    public string? Endpoint { get; set; }
    public string? ApiKey { get; set; }
    public string DeploymentName { get; set; } = "gpt-4o-mini";

    /// <summary>
    /// Checks if a valid, non-placeholder Azure OpenAI configuration is present.
    /// </summary>
    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(Endpoint) &&
        !string.IsNullOrWhiteSpace(ApiKey) &&
        !Endpoint.Equals("url", StringComparison.OrdinalIgnoreCase) &&
        !ApiKey.Equals("openapikey", StringComparison.OrdinalIgnoreCase) &&
        !ApiKey.Equals("openaikey", StringComparison.OrdinalIgnoreCase) &&
        Uri.TryCreate(Endpoint, UriKind.Absolute, out _);
}
