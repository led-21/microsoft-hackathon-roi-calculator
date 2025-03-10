# Microsoft Hackathon ROI Calculator
Quando as empresas precisam se adaptar, a mudança geralmente traz riscos substanciais, e 70% dos projetos de transformação falham. Estamos procurando mudar essa estatística usando IA para permitir que os líderes tomem decisões mais inteligentes. A prontidão para a transformação é fundamental. Se as equipes não estiverem preparadas, as iniciativas enfrentam lançamentos atrasados, desperdício de recursos e funcionários frustrados e desengajados. 

## Desafio 
Construir uma calculadora de ROI para ajudar os líderes a se prepararem para a mudança com insights preditivos, recomendações acionáveis para iniciativas de mudança, visualizações e relatórios. Entradas de amostra incluiriam orçamento do projeto, número de funcionários impactados e duração do projeto. Considere os riscos financeiros de falha do projeto e desengajamento dos funcionários, bem como as economias potenciais de produtividade aumentada, redução de risco e entrega bem-sucedida do projeto.

## Entradas
- Orçamento do projeto em reais
- Número de funcionários impactados
- Duração do projeto em meses

Considerar:
- Riscos financeiros de falha do projeto
- Desengajamento dos funcionários
- Economias potenciais de produtividade aumentada
- Redução de risco
- Entrega bem-sucedida do projeto

## Retorno sobre Investimento (ROI)

O ROI é uma métrica financeira usada para avaliar a eficiência ou lucratividade de um investimento, comparando os benefícios gerados (ganhos ou economias) com o custo do investimento.

### Fórmula do ROI

$$\text{ROI (\%)} = \left[\frac{\text{Benefícios Totais} - \text{Investimento Total}}{\text{Investimento Total}}\right] \times 100$$

#### Onde:
- **Benefícios Totais**: Soma das economias projetadas (aumento de produtividade, redução de riscos, sucesso do projeto).
- **Investimento Total**: Custo total do projeto, incluindo orçamento, mitigação de riscos e outros gastos relacionados.

### Componentes dos Cálculos

#### Custo Médio Mensal por Funcionário

$$\text{averageEmployeeCostPerMonth} = \frac{\text{input.ProjectBudget}}{\text{input.NumberOfEmployees} \times \text{input.ProjectDurationMonths}}$$

#### 1. Economia por Produtividade Aumentada

##### Ganho Bruto:

$$\text{ProductivityGainValue} = (\text{AverageEmployeeCostPerMonth} \times (\text{ExpectedProductivityGain} - 1)) \times \text{NumberOfEmployees} \times \text{ProjectDurationMonths}$$

##### Ganho Ajustado:

$$\text{AdjustedProductivityGain} = \text{ProductivityGainValue} \times (1 - \text{ExpectedDisengagementRate})$$

#### 2. Redução de Risco

##### Valor Bruto:

$$\text{RiskReductionValue} = (\text{ProjectBudget} \times \text{BudgetLossRate}) \times \text{ProjectedRiskReduction}$$

#### Valor Ajustado:

$$\text{AdjustedRiskReduction} = \text{RiskReductionValue} \times (1 - \text{FailureRate})$$

#### 3. Benefício de Sucesso

##### Valor Bruto:

$$\text{SuccessBenefitValue} = \text{ProjectBudget} \times \text{SuccessBenefit}$$

##### Valor Ajustado:

$$\text{AdjustedSuccessBenefit} = \text{SuccessBenefitValue} \times (1 - \text{FailureRate})$$

#### Soma dos Benefícios Ajustados

$$\text{TotalBenefits} = \text{AdjustedProductivityGain} + \text{AdjustedRiskReduction} + \text{AdjustedSuccessBenefit}$$
