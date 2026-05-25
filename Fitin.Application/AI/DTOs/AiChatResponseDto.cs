namespace Fitin.Application.AI.DTOs;

public class AiChatResponseDto
{
    public string Answer { get; set; } = string.Empty;
    public List<AiSuggestedProductDto> SuggestedProducts { get; set; } = [];
    public List<AiSourceDto> Sources { get; set; } = [];
}