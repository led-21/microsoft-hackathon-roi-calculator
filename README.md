# Microsoft Hackathon ROI Calculator
Quando as empresas precisam se adaptar, a mudança geralmente traz riscos substanciais, e 70% dos projetos de transformação falham. Estamos procurando mudar essa estatística usando IA para permitir que os líderes tomem decisões mais inteligentes. A prontidão para a transformação é fundamental. Se as equipes não estiverem preparadas, as iniciativas enfrentam lançamentos atrasados, desperdício de recursos e funcionários frustrados e desengajados. 

## Desafio 
Construir uma calculadora de ROI para ajudar os líderes a se prepararem para a mudança com insights preditivos, recomendações acionáveis para iniciativas de mudança, visualizações e relatórios. Entradas de amostra incluiriam orçamento do projeto, número de funcionários impactados e duração do projeto. Considere os riscos financeiros de falha do projeto e desengajamento dos funcionários, bem como as economias potenciais de produtividade aumentada, redução de risco e entrega bem-sucedida do projeto.

# Cálculo de Retorno sobre Investimento (ROI) para Projetos
Este documento descreve um modelo para calcular o Retorno sobre Investimento (ROI) de um projeto, considerando fatores como orçamento, impacto nos funcionários, duração, riscos financeiros, produtividade e sucesso na entrega. O objetivo é fornecer uma análise quantitativa que auxilie na avaliação da viabilidade e lucratividade do investimento.

## Entradas
- **Orçamento do projeto**: Valor total alocado para o projeto.
- **Número de funcionários impactados:** Quantidade de colaboradores afetados diretamente pelo projeto.
- **Duração do projeto (em meses):** Período estimado para a execução completa.

## Fatores a Considerar
- **Riscos financeiros de falha:** Perdas potenciais caso o projeto não seja concluído com sucesso.
- **Desengajamento dos funcionários:** Impacto da falta de motivação ou adesão da equipe.
- **Economias por produtividade aumentada:** Ganhos estimados com o aumento da eficiência dos funcionários.
- **Redução de risco:** Benefícios associados à mitigação de incertezas ou falhas.
- **Entrega bem-sucedida:** Valor agregado pela conclusão do projeto dentro dos objetivos planejados.

## Retorno sobre Investimento (ROI)

O ROI é uma métrica financeira usada para avaliar a eficiência ou lucratividade de um investimento, comparando os benefícios gerados (ganhos ou economias) com o custo do investimento.

### Fórmula do ROI

$$\text{ROI \%)} = \left[\frac{\text{Benefícios Totais} - \text{Investimento Total}}{\text{Investimento Total}}\right] \times 100$$

#### Onde:
- **Benefícios Totais**: Soma das economias projetadas (aumento de produtividade, redução de riscos e sucesso do projeto).
- **Investimento Total**: Custo total do projeto, incluindo orçamento, mitigação de riscos e outros gastos relacionados.

### Componentes dos Cálculos

#### 1. Custo Médio Mensal por Funcionário

$$\text{Custo Médio por Funcionário} = \frac{\text{Orçamento do Projeto}}{\text{Número de Funcionários} \times \text{Duração do Projeto em Meses}}$$

#### 2. Economia por Produtividade Aumentada

$$\text{Ganho de Produtividade} = (\text{Custo Médio Mensal por Funcionário} \times (\text{Ganho de Produtividade Esperado} - 1)) \times \text{Número de Funcionários} \times \text{Duração do Projeto em Meses}$$

$$\text{Ganho de Produtividade Ajustado} = \text{Ganho de Produtividade} \times (1 - \text{Taxa de Desenganjamento Esperada})$$

#### 3. Redução de Risco

$$\text{Redução de Risco} = (\text{Orçamento do Projeto} \times \text{Taxa de Perda do Orçamento em caso de falha}) \times \text{Redução de Risco Projetada}$$

$$\text{Redução de Risco Ajustado} = \text{Redução de Risco} \times (1 - \text{Taxa de Falha})$$

#### 4. Benefício de Sucesso

$$\text{Benefícios de Sucesso} = \text{Orçamento do Projeto} \times \text{Fator de Benefícios de Sucesso}$$

$$\text{Benefícios de Sucesso Ajustado} = \text{Benefícios de Sucesso} \times (1 - \text{Taxa de Falha})$$

#### Soma dos Benefícios Ajustados

$$\text{Benefícios Totais} = \text{Ganho de Produtividade Ajustado} + \text{Redução de Risco Ajustado} + \text{Benefícios de Sucesso Ajustado}$$
