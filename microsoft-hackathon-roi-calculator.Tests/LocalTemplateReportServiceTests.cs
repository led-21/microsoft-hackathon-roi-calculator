using microsoft_hackathon_roi_calculator.Application.Interfaces;
using microsoft_hackathon_roi_calculator.Application.Services;

namespace microsoft_hackathon_roi_calculator.Tests;

/// <summary>
/// Tests for <see cref="LocalTemplateReportService"/>, the deterministic offline
/// insights generator that produces executive analysis based on report content.
/// </summary>
public class LocalTemplateReportServiceTests
{
    private readonly LocalTemplateReportService _sut = new();

    // ──────────────────────────────────────────────
    //  1. Positive viability path ("Positiva" present)
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GenerateInsightsAsync_ReportContainsPositiva_ReturnsPositiveViabilityText()
    {
        // Arrange
        const string report = "Viabilidade: Positiva — ROI projetado de 150%.";

        // Act
        var result = await _sut.GenerateInsightsAsync(report);

        // Assert
        Assert.Contains("favorável com retorno projetado positivo", result);
    }

    [Theory]
    [InlineData("positiva")]
    [InlineData("POSITIVA")]
    [InlineData("PoSiTiVa")]
    public async Task GenerateInsightsAsync_ReportContainsPositivaCaseInsensitive_ReturnsPositiveViabilityText(string keyword)
    {
        // Arrange
        var report = $"Viabilidade: {keyword}";

        // Act
        var result = await _sut.GenerateInsightsAsync(report);

        // Assert
        Assert.Contains("favorável com retorno projetado positivo", result);
    }

    // ──────────────────────────────────────────────
    //  2. Negative / alert path ("Positiva" absent)
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GenerateInsightsAsync_ReportDoesNotContainPositiva_ReturnsAlertViabilityText()
    {
        // Arrange
        const string report = "Viabilidade: Negativa — ROI projetado de -10%.";

        // Act
        var result = await _sut.GenerateInsightsAsync(report);

        // Assert
        Assert.Contains("em zona de alerta financeiro e risco de execução", result);
    }

    [Fact]
    public async Task GenerateInsightsAsync_ReportDoesNotContainPositiva_DoesNotContainPositiveText()
    {
        // Arrange
        const string report = "Relatório sem a palavra-chave esperada.";

        // Act
        var result = await _sut.GenerateInsightsAsync(report);

        // Assert
        Assert.DoesNotContain("favorável com retorno projetado positivo", result);
    }

    // ──────────────────────────────────────────────
    //  3. Non-null, non-empty return value
    // ──────────────────────────────────────────────

    [Theory]
    [InlineData("Viabilidade: Positiva")]
    [InlineData("Viabilidade: Negativa")]
    [InlineData("Qualquer texto")]
    public async Task GenerateInsightsAsync_AnyInput_ReturnsNonNullNonEmptyString(string report)
    {
        // Act
        var result = await _sut.GenerateInsightsAsync(report);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    // ──────────────────────────────────────────────
    //  4. Empty input
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GenerateInsightsAsync_EmptyInput_ReturnsValidOutput()
    {
        // Arrange — empty string has no "Positiva", so negative path is expected
        const string report = "";

        // Act
        var result = await _sut.GenerateInsightsAsync(report);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains("em zona de alerta financeiro e risco de execução", result);
    }

    // ──────────────────────────────────────────────
    //  5. Null input — throws NullReferenceException
    //     because string.Contains is called on null
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GenerateInsightsAsync_NullInput_ThrowsNullReferenceException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(
            () => _sut.GenerateInsightsAsync(null!));
    }

    // ──────────────────────────────────────────────
    //  6. CancellationToken behavior
    //     The current implementation ignores the token
    //     (synchronous Task.FromResult), so it completes
    //     even with an already-cancelled token.
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GenerateInsightsAsync_CancelledToken_CompletesWithoutThrowing()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        const string report = "Viabilidade: Positiva";

        // Act — should complete because implementation does not observe the token
        var result = await _sut.GenerateInsightsAsync(report, cts.Token);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GenerateInsightsAsync_DefaultCancellationToken_CompletesSuccessfully()
    {
        // Arrange
        const string report = "Viabilidade: Positiva";

        // Act
        var result = await _sut.GenerateInsightsAsync(report, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("favorável com retorno projetado positivo", result);
    }

    // ──────────────────────────────────────────────
    //  7. Structural sections present in output
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GenerateInsightsAsync_AnyInput_ContainsExecutiveAnalysisHeader()
    {
        // Act
        var result = await _sut.GenerateInsightsAsync("test");

        // Assert
        Assert.Contains("Análise Executiva de Transformação e Prontidão", result);
    }

    [Fact]
    public async Task GenerateInsightsAsync_AnyInput_ContainsResumoExecutivoSection()
    {
        // Act
        var result = await _sut.GenerateInsightsAsync("test");

        // Assert
        Assert.Contains("#### 1. Resumo Executivo", result);
    }

    [Fact]
    public async Task GenerateInsightsAsync_AnyInput_ContainsDiagnosticoDeRiscoSection()
    {
        // Act
        var result = await _sut.GenerateInsightsAsync("test");

        // Assert
        Assert.Contains("#### 2. Diagnóstico de Risco e Sensibilidade", result);
    }

    [Fact]
    public async Task GenerateInsightsAsync_AnyInput_ContainsRecomendacoesSection()
    {
        // Act
        var result = await _sut.GenerateInsightsAsync("test");

        // Assert
        Assert.Contains("#### 3. Recomendações Estratégicas para Líderes de Mudança", result);
    }

    [Theory]
    [InlineData("Governança por Ondas")]
    [InlineData("Plano de Gestão de Mudança e Engajamento")]
    [InlineData("Métricas de Adoção em Tempo Real")]
    [InlineData("Comitê de Mitigação de Riscos")]
    public async Task GenerateInsightsAsync_AnyInput_ContainsStrategicRecommendation(string recommendation)
    {
        // Act
        var result = await _sut.GenerateInsightsAsync("test");

        // Assert
        Assert.Contains(recommendation, result);
    }

    [Fact]
    public async Task GenerateInsightsAsync_AnyInput_ContainsLocalModeDisclaimer()
    {
        // Act
        var result = await _sut.GenerateInsightsAsync("test");

        // Assert
        Assert.Contains("motor de inteligência analítica local do InovaROI", result);
    }

    // ──────────────────────────────────────────────
    //  Interface conformance
    // ──────────────────────────────────────────────

    [Fact]
    public void LocalTemplateReportService_ImplementsIAssistantReportService()
    {
        // Assert
        Assert.IsAssignableFrom<IAssistantReportService>(_sut);
    }
}
