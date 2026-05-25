using Fitin.Application.AI.DTOs;

namespace Fitin.Application.AI.Interfaces;

public interface IAiChatService
{
    Task<AiChatResponseDto> ChatAsync(AiChatRequestDto request);
}