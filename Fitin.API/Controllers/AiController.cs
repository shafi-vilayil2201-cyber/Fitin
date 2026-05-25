using Fitin.Application.AI.DTOs;
using Fitin.Application.AI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/ai")]
public class AiController : BaseApiController
{
    private readonly IAiChatService _aiChatService;
    private readonly ILogger<AiController> _logger;

    public AiController(IAiChatService aiChatService, ILogger<AiController> logger)
    {
        _aiChatService = aiChatService;
        _logger = logger;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] AiChatRequestDto request)
    {
        try
        {
            var result = await _aiChatService.ChatAsync(request);
            return Success(result, "AI response generated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI chat request failed.");
            return Failure("AI assistant is temporarily unavailable. Please try again later.", statusCode: 503);
        }
    }
}