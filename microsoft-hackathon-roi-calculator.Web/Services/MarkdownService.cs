using Markdig;

namespace microsoft_hackathon_roi_calculator.Web.Services
{
    public class MarkdownService
    {
        public string ConvertToHtml(string markdownText)
        {
            if (string.IsNullOrEmpty(markdownText))
                return string.Empty;

            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

            return Markdown.ToHtml(markdownText, pipeline);
        }
    }
}
