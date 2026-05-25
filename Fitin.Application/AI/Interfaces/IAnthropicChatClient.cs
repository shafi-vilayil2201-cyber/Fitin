namespace Fitin.Application.AI.Interfaces;

public interface IAnthropicChatClient
{
    Task<string> GetCompletionAsync(string systemPrompt, string userPrompt);
}