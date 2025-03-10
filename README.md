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

### Fórmula do ROI (Retorno sobre Investimento)

**ROI (%) = [(Benefícios Totais - Investimento Total) / Investimento Total] × 100**

### Onde:
- **Benefícios Totais**: Soma das economias projetadas (aumento de produtividade, redução de riscos, sucesso do projeto).
- **Investimento Total**: Custo total do projeto, incluindo orçamento, mitigação de riscos e outros gastos relacionados.

### Componentes dos Cálculos

#### 1. Custo Médio Mensal por Funcionário
**Custo Médio Mensal por Funcionário**  
`AverageEmployeeCostPerMonth = ProjectBudgetNumberOfEmployees × ProjectDurationMonths`

#### 2. Economia por Produtividade Aumentada
- **Ganho Bruto:**  
`ProductivityGainValue = (AverageEmployeeCostPerMonth × (ExpectedProductivityGain − 1)) × NumberOfEmployees × ProjectDurationMonths`

- **Ganho Ajustado:**  
`AdjustedProductivityGain = ProductivityGainValue × (1 − ExpectedDisengagementRate)`

#### 3. Redução de Risco
- **Valor Bruto:**  
`RiskReductionValue = (ProjectBudget × BudgetLossRate) × ProjectedRiskReduction`

- **Valor Ajustado:**  
`AdjustedRiskReduction = RiskReductionValue × (1 − FailureRate)`

#### 4. Benefício de Sucesso
- **Valor Bruto:**  
`SuccessBenefitValue = ProjectBudget × SuccessBenefit`

- **Valor Ajustado:**  
`AdjustedSuccessBenefit = SuccessBenefitValue × (1 − FailureRate)`

#### 5. Soma dos Benefícios Ajustados
`TotalBenefits = AdjustedProductivityGain + AdjustedRiskReduction + AdjustedSuccessBenefit`




















