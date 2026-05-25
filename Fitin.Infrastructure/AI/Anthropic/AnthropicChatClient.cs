using Anthropic;
using Anthropic.Models.Messages;
using Fitin.Application.AI.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Fitin.Infrastructure.AI.Anthropic;

public class AnthropicChatClient : IAnthropicChatClient
{
    private readonly AnthropicClient _client;
    private readonly string _model;

    public AnthropicChatClient(IConfiguration configuration)
    {
        var apiKey = configuration["Anthropic:ApiKey"]
            ?? throw new InvalidOperationException("Anthropic API key is missing.");

        _model = configuration["Anthropic:Model"]
            ?? "claude-sonnet-4-20250514";

        _client = new AnthropicClient
        {
            ApiKey = apiKey
        };
    }

    public async Task<string> GetCompletionAsync(string systemPrompt, string userPrompt)
    {
        MessageCreateParams parameters = new()
        {
            Model = _model,
            MaxTokens = 1000,
            System = systemPrompt,
            Messages =
            [
                new()
                {
                    Role = Role.User,
                    Content = userPrompt
                }
            ]
        };

        var message = await _client.Messages.Create(parameters);

        var textParts = new List<string>();

        foreach (var block in message.Content)
        {
            if (block.TryPickText(out var textBlock) && !string.IsNullOrWhiteSpace(textBlock.Text))
            {
                textParts.Add(textBlock.Text);
            }
        }

        return string.Join("\n", textParts);
    }
}