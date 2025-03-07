using microsoft_hackathon_roi_calculator.Core.Models.Metrics;

namespace microsoft_hackathon_roi_calculator.Web.Services
{
    public class ROIApiService
    {
        private readonly HttpClient _httpClient;

        public ROIApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> CalculateROI(CostBenefitMetrics metrics)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/roi", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<decimal>();
        }

        public async Task<decimal> CalculateProcessComplianceRate(ProcessMetrics metrics)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/process-compliance", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<decimal>();
        }

        public async Task<decimal> CalculateTrainingCompletionRate(TrainingMetrics metrics)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/training-completion", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<decimal>();
        }

        public async Task<decimal> CalculateEmployeeAdoptionRate(EmployeeMetrics metrics)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/employee-adoption", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<decimal>();
        }

        public async Task<decimal> CalculateAverageScore(ResponseMetrics metrics)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/average-score", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<decimal>();
        }

        public async Task<decimal> CalculatePositiveResponseRate(ResponseMetrics metrics)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/positive-response", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<decimal>();
        }

        public async Task<int> CalculateTrainingScoreImprovement(TrainingMetrics metrics)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/training-score-improvement", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task<decimal> CalculateImplementationEfficiency(ImplementationMetrics metrics)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/implementation-efficiency", metrics);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<decimal>();
        }
    }
}
