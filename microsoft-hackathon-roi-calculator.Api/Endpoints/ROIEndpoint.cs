using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Globalization;
using microsoft_hackathon_roi_calculator.Domain.Models;
using microsoft_hackathon_roi_calculator.Persistence.Data;
using microsoft_hackathon_roi_calculator.Application.Interfaces;
using OllamaSharp;
using OllamaSharp.Models.Chat;
using Azure.AI.OpenAI;
using Azure;

namespace microsoft_hackathon_roi_calculator.Api.Endpoints;
static public class ROIEndpoint
{
    static AzureOpenAIClient _openAIClient = new AzureOpenAIClient(new Uri("url"), new AzureKeyCredential("openapikey"));
    static public void AddROIEndpoint(this WebApplication app)
    {
        app.MapPost("/api/roi/ai", async (HttpRequest request, IOllamaApiClient apiClient) =>
        {

            string report = await new StreamReader(request.Body).ReadToEndAsync();

            var prompt = $"""
                Com base nos seguintes resultados de cálculo de ROI, forneça insights e recomendações:
                
                Estrutura do Relatório Final

                Resumo Executivo: Visão geral.
                Análise Detalhada: Explicação dos cálculos e dados analisados.
                Insights e Recomendações: Conclusões acionáveis baseadas na análise.

                Relatorio: {report}
                """;

            ChatRequest chatRequest = new()
            {
                Messages = [new Message() {
                    Role = ChatRole.User,
                    Content = prompt

                }]
            };

            string result = "";

            try
            {
                // OpenAI API call
                result = _openAIClient.GetChatClient("gpt-4o")
                                          .CompleteChatAsync(prompt)
                                          .ConfigureAwait(false)
                                          .GetAwaiter()
                                          .GetResult()
                                          .Value
                                          .Content[0]
                                          .Text;
            }
            catch (Exception ex)
            {
                return Results.Problem($"Ocorreu um erro ao processar a solicitação do gerador de relatorio por AI. \n{ex.Message}");
            }

            return Results.Content(result, "text/plain");

        }).WithRequestTimeout(TimeSpan.FromMinutes(1));


        app.MapPost("/api/roi/calculator", (IROICalculatorService calculator, ROIInputParameters input) =>
    {
        string report = string.Empty;

        try
        {
            var result = calculator.CalculateROI(input);
            report = calculator.GenerateReport(result, input) + "\n";
        }
        catch (Exception ex)
        {
            report = "Error: " + ex.Message;
            return Results.BadRequest(report);
        }

        return Results.Content(report, "text/plain");
    });

        app.MapPost("/api/roi/estimate/failure-rate", (IROICalculatorService calculator, ROIInputParameters input) =>
        {
            try
            {
                var estimate = calculator.EstimateFailureRate(input);

                if (double.IsNaN(estimate))
                    return Results.BadRequest();

                return Results.Ok(estimate.ToString("0.00") + "%");
            }
            catch
            {
                return Results.BadRequest("Erro ao calcular taxa de falha.");
            }
        });

        app.MapGet("/api/roi/csv", async (CalculatorDbContext dbContext) =>
        {
            try
            {
                var roiList = await dbContext.ProjectROIs.ToListAsync();
                var csv = new StringBuilder();
                csv.AppendLine("ProjectName,ProjectBudget,NumberOfEmployees,ProjectDurationMonths,ROI");

                foreach (var roi in roiList)
                {
                    csv.AppendLine($"{roi.ProjectName},{roi.ProjectBudget.ToString(CultureInfo.InvariantCulture)},{roi.NumberOfEmployees},{roi.ProjectDurationMonths},{roi.ROI.ToString(CultureInfo.InvariantCulture)}");
                }

                var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
                return Results.File(csvBytes, "text/csv", "roi.csv");
            }
            catch (Exception ex)
            {
                // Log the exception
                return Results.Problem($"Ocorreu um erro ao gerar o arquivo CSV.  \n{ex.Message}");
            }
        });

        app.MapGet("/api/roi", async (CalculatorDbContext dbContext, IDistributedCache cache) =>
        {
            try
            {
                var cachedRoi = cache.Get("roi");

                if (cachedRoi == null)
                {
                    var roi = await dbContext.ProjectROIs.ToListAsync();

                    await cache.SetAsync("roi", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(roi)), new()
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(10)
                    });

                    return Results.Ok(roi);
                }

                return Results.Ok(JsonSerializer.Deserialize<IEnumerable<ProjectROI>>(cachedRoi));
            }
            catch (Exception ex)
            {
                // Log the exception
                return Results.Problem($"Ocorreu um erro ao recuperar os dados de ROI.  \n{ex.Message}");
            }
        });

        app.MapPost("/api/roi", async (CalculatorDbContext dbContext, ProjectROI newROI) =>
        {
            try
            {
                dbContext.ProjectROIs.Add(newROI);
                await dbContext.SaveChangesAsync();

                return Results.Created($"/api/roi/{newROI.Id}", newROI);
            }
            catch (Exception ex)
            {
                // Log the exception
                return Results.Problem($"Ocorreu um erro ao criar a nova entrada de ROI.  \n{ex.Message}");
            }
        });

        app.MapPut("/api/roi/{id}", async (CalculatorDbContext dbContext, int id, ProjectROI updatedROI) =>
        {
            try
            {
                var existingROI = await dbContext.ProjectROIs.FindAsync(id);

                if (existingROI == null)
                    return Results.NotFound();

                existingROI.ProjectName = updatedROI.ProjectName;
                existingROI.ProjectBudget = updatedROI.ProjectBudget;
                existingROI.NumberOfEmployees = updatedROI.NumberOfEmployees;
                existingROI.ProjectDurationMonths = updatedROI.ProjectDurationMonths;

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return Results.Problem($"Ocorreu um erro ao atualizar a entrada de ROI.  \n{ex.Message}");
            }
        });

        app.MapDelete("/api/roi/{id}", async (CalculatorDbContext dbContext, int id) =>
        {
            try
            {
                var existingROI = await dbContext.ProjectROIs.FindAsync(id);

                if (existingROI == null)
                    return Results.NotFound();

                dbContext.ProjectROIs.Remove(existingROI);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return Results.Problem($"Ocorreu um erro ao excluir a entrada de ROI. \n{ex.Message}");
            }
        });
    }
}