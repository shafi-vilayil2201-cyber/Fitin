using Fitin.Application.AI.DTOs;

namespace Fitin.Application.AI.Interfaces;

public interface IProductAssistantService
{
    Task<List<AiSuggestedProductDto>> FindRelevantProductsAsync(string userMessage);
}