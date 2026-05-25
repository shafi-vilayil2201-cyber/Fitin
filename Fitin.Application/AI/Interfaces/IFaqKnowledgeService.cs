using Fitin.Application.AI.DTOs;

namespace Fitin.Application.AI.Interfaces;

public interface IFaqKnowledgeService
{
    Task<List<AiDocumentChunkDto>> SearchRelevantChunksAsync(string query);
}