namespace Fitin.Application.AI.DTOs;

public class AiChatRequestDto
{
    public string Message { get; set; } = string.Empty;
    public string? ConversationId { get; set; }
    public string? PageContext { get; set; }
}