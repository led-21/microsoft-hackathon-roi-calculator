using microsoft_hackathon_roi_calculator.Application.Interfaces;

namespace microsoft_hackathon_roi_calculator.Application.Services;

/// <summary>
/// Deterministic, local generator providing executive insights and change-readiness analysis.
/// Serves as the zero-latency, offline alternative when cloud AI services are unavailable.
/// </summary>
public class LocalTemplateReportService : IAssistantReportService
{
    public Task<string> GenerateInsightsAsync(string reportMarkdown, CancellationToken cancellationToken = default)
    {
        bool isPositive = reportMarkdown.Contains("Positiva", StringComparison.OrdinalIgnoreCase);

        var insights = $"""

### Análise Executiva de Transformação e Prontidão (Modo Local / IA)

#### 1. Resumo Executivo
Com base nas premissas modeladas, a iniciativa apresenta viabilidade **{(isPositive ? "favorável com retorno projetado positivo" : "em zona de alerta financeiro e risco de execução")}**.
A prontidão organizacional e a taxa de adoção das novas ferramentas serão os principais determinantes para a captura real do valor estimado.

#### 2. Diagnóstico de Risco e Sensibilidade
- **Risco de Desengajamento:** O desengajamento da equipe impacta diretamente a captura de ganhos de produtividade. Projetos com fricção cultural tendem a sofrer degradação de até 20% a 30% no valor planejado.
- **Risco de Execução (Taxa de Falha):** A taxa de falha de referência corrobora que 70% das iniciativas de transformação enfrentam atrasos ou interrupções caso marcos intermediários não sejam rigidamente monitorados.
- **Ponto de Equilíbrio (Break-Even):** Para preservar a viabilidade positiva, o projeto deve manter a adesão às novas práticas acima do limiar crítico estabelecido na simulação.

#### 3. Recomendações Estratégicas para Líderes de Mudança
1. **Governança por Ondas (Phased Rollout):** Implementar a solução em grupos-piloto antes da escala geral, validando as premissas de ganho de eficiência com métricas reais.
2. **Plano de Gestão de Mudança e Engajamento:** Realizar workshops focados nas dores do usuário final para reduzir a resistência e acelerar a curva de aprendizado.
3. **Métricas de Adoção em Tempo Real:** Monitorar a proporção de colaboradores utilizando a ferramenta ativamente para antecipar desvios operacionais.
4. **Comitê de Mitigação de Riscos:** Estabelecer revisões quinzenais de progresso orçamentário e cumprimento de cronograma para evitar desvios no investimento.

*(Nota: Relatório gerado pelo motor de inteligência analítica local do InovaROI — operação autônoma sem dependência de serviços externos).*
""";

        return Task.FromResult(insights);
    }
}
