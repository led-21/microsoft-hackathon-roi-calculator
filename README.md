# Microsoft Hackathon ROI Calculator
Quando as empresas precisam se adaptar, a mudança geralmente traz riscos substanciais, e 70% dos projetos de transformação falham. Estamos procurando mudar essa estatística usando IA para permitir que os líderes tomem decisões mais inteligentes. A prontidão para a transformação é fundamental. Se as equipes não estiverem preparadas, as iniciativas enfrentam lançamentos atrasados, desperdício de recursos e funcionários frustrados e desengajados. 

## Desafio 
Construir uma calculadora de ROI para ajudar os líderes a se prepararem para a mudança com insights preditivos, recomendações acionáveis para iniciativas de mudança, visualizações e relatórios. Entradas de amostra incluiriam orçamento do projeto, número de funcionários impactados e duração do projeto. Considere os riscos financeiros de falha do projeto e desengajamento dos funcionários, bem como as economias potenciais de produtividade aumentada, redução de risco e entrega bem-sucedida do projeto.

## Retorno sobre Investimento (ROI)

O ROI é uma métrica financeira usada para avaliar a eficiência ou lucratividade de um investimento, comparando os benefícios gerados (ganhos ou economias) com o custo do investimento.

### Fórmula do ROI

$$\text{ROI \%)} = \left[\frac{\text{Benefícios Totais} - \text{Investimento Total}}{\text{Investimento Total}}\right] \times 100$$

#### Onde:
- **Benefícios Totais**: Soma das economias projetadas (aumento de produtividade, redução de riscos e sucesso do projeto).
- **Investimento Total**: Custo total do projeto, incluindo orçamento, mitigação de riscos e outros gastos relacionados.

[Métodos de Cálculos Utilizados no Aplicativo](docs/componentes-de-calculo.md)

# Arquitetura de Serviços Azure
![Arquitetura de Serviços](docs/arquitetura.gif)

## Componentes Principais

### Orquestrador
- **.NET Aspire**:  
  Atua como o orquestrador central da aplicação, gerenciando a integração e a comunicação entre todos os componentes distribuídos. Simplifica o desenvolvimento, teste e deploy ao coordenar o frontend, backend, serviços serverless e conexões com o Azure.

### Frontend
- **Blazor**:  
  Responsável pela interface de usuário (UI). Utilizado para:  
  - Entrada de dados e exibição dos resultados de cálculo da calculadora de ROI.  
  - Cadastro de resultados de projetos concluídos para alimentar os modelos de Machine Learning.  
  - Visualização interativa e dinâmica de gráficos, dados de projetos e relatórios personalizados, proporcionando uma experiência rica e intuitiva ao usuário.

### Backend
- **C# / .NET**:  
  Camada responsável por processar a lógica de negócios. Realiza operações CRUD (Create, Read, Update, Delete) no banco de dados Azure SQL, garantindo a integridade e consistência dos dados.

### Banco de Dados
- **Azure SQL Database**:  
  Central de armazenamento de dados da aplicação. Armazena informações sobre projetos, incluindo:
  - **Identificação e Descrição**:  
    - `id`: Chave primária única para cada projeto.  
    - `project_name`: Nome do projeto (obrigatório).  
    - `description`: Descrição detalhada do projeto (opcional).  
  - **Financeiro e Temporal**:  
    - `project_budget`: Orçamento do projeto.  
    - `start_date`: Data de início do projeto.  
    - `project_duration_months`: Duração estimada do projeto em meses.  
    - `roi`: Retorno sobre o investimento calculado.  
  - **Equipe e Impacto**:  
    - `number_of_employees`: Número de funcionários impactados pelo projeto.  
    - `employees_using_new_tool`: Quantidade de funcionários utilizando novas ferramentas introduzidas.
    - `total_hours_worked_weekly`: Total de horas trabalhadas por semana pelos funcionários no projeto.  
    - `completed_training`: Número de treinamentos concluídos pelos funcionários no projeto.  
  - **Métricas de Mundaças**:  
    - `total_change_implementation_time`: Tempo total de implementação de mudanças.  
    - `total_planned_implementation_time`: Tempo planejado para implementação.  
  - **Qualidade e Conformidade**:  
    - `total_processes`: Total de processos envolvidos no projeto.  
    - `compliant_processes`: Número de processos em conformidade.  
  - **Avaliação do Projeto**:  
    - `project_evaluation_total_responses`: Total de respostas na avaliação do projeto.  
    - `project_evaluation_positive_responses`: Número de respostas positivas na avaliação.  
    - `project_evaluation_sum_of_all_scores`: Soma de todas as pontuações da avaliação.  
### Machine Learning
- **Azure Machine Learning**:  
  Consome os dados do Azure SQL Database para treinar modelos preditivos. Esses modelos são utilizados para:
  - Estimar o ROI do projeto e essa infromação é usado para calcular o riscos de projetos.
  
  Os modelos são ajustados dinamicamente conforme novos dados são incorporados, assegurando relevância e precisão contínuas nas previsões.

### Funções Serverless
- **Azure Functions**:  
  Implementa cálculos de ROI em tempo real de forma eficiente e escalável. Além disso, integra-se ao OpenAI para a geração de relatórios personalizados sob demanda.

### Relatórios Personalizados
- **OpenAI**:  
  Utilizado para criar relatórios detalhados e análises interpretativas baseadas nos dados processados, oferecendo insights valiosos aos usuários em linguagem natural.

### Hospedagem
- **Azure Web App**:  
  Plataforma de hospedagem da aplicação web. Garante:
  - Escalabilidade automática
  - Alta disponibilidade
  - Estabilidade do ambiente de produção

## Fluxo Geral
1. O usuário interage com a interface em **Blazor** para inserir dados de cálculo e projetos.
2. O backend em **C# / .NET** processa as informações e realiza operações no **Azure SQL Database**.
3. O **Azure Machine Learning** utiliza os dados armazenados para treinar e ajustar modelos preditivos.
4. O **Azure Functions** calcula o ROI e outros indices em tempo real e aciona o **OpenAI** para gerar relatórios personalizados.
5. Os resultados são exibidos ao usuário na interface **Blazor**, hospedada no **Azure Web App**.

## Benefícios da Arquitetura
- **Escalabilidade**: Uso de serviços Azure como Web App e Functions permite lidar com picos de demanda.
- **Flexibilidade**: Integração com Machine Learning e OpenAI possibilita adaptação a diferentes casos de uso.
- **Eficiência**: Funções serverless reduzem custos operacionais ao executar apenas sob demanda.
- **Manutenção Simplificada**: Banco de dados e hospedagem gerenciados pelo Azure minimizam a necessidade de gerenciamento manual.

## Limitações
- Os dados atualmente utilizados são gerados de forma aleatória apenas para fins de teste, apresentando baixa significância estatística e não refletindo cenários reais de uso.
  
## Próximos Passos
- Implementar autenticação e autorização para garantir segurança dos dados.
- Adicionar dados reais de projeto para o treinamento dos modelos de Machine Learning.
- Adicionar testes automatizados para os modelos de Machine Learning.

## Equipe do Projeto

Desenvolvida por uma equipe dedicada para competir no **Microsoft Innovation Challenge Hackathon March 2025**, esta aplicação visa transformar a estatística de fracasso em sucesso, capacitando empresas a se adaptarem com confiança e eficiência.

### Membros
- **Adriano Godoy** [led-21](https://github.com/led-21/)  
- **Danillo Silva** [DanilloAraujo](https://github.com/DanilloAraujo)  
