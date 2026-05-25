using Fitin.Application.AI.DTOs;
using Fitin.Application.AI.Interfaces;

namespace Fitin.Application.AI.Services;

public class FaqKnowledgeService : IFaqKnowledgeService
{
    private readonly string _knowledgePath;

    public FaqKnowledgeService()
    {
        _knowledgePath = Path.Combine(AppContext.BaseDirectory, "AI", "Knowledge");
    }

    public async Task<List<AiDocumentChunkDto>> SearchRelevantChunksAsync(string query)
    {
        if (!Directory.Exists(_knowledgePath))
            return [];

        var files = Directory.GetFiles(_knowledgePath, "*.md");
        var keywords = query
            .ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var chunks = new List<AiDocumentChunkDto>();

        foreach (var file in files)
        {
            var content = await File.ReadAllTextAsync(file);
            var score = keywords.Count(keyword => content.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            if (score > 0)
            {
                chunks.Add(new AiDocumentChunkDto
                {
                    Title = Path.GetFileNameWithoutExtension(file),
                    Source = Path.GetFileName(file),
                    Content = content
                });
            }
        }

        return chunks
            .OrderByDescending(chunk =>
                keywords.Count(keyword => chunk.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            .Take(3)
            .ToList();
    }
}