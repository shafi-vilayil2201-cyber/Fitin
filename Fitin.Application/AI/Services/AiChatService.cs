using System.Text;
using Fitin.Application.AI.DTOs;
using Fitin.Application.AI.Interfaces;

namespace Fitin.Application.AI.Services;

public class AiChatService : IAiChatService
{
    private readonly IAnthropicChatClient _anthropicChatClient;
    private readonly IFaqKnowledgeService _faqKnowledgeService;
    private readonly IProductAssistantService _productAssistantService;

    public AiChatService(
        IAnthropicChatClient anthropicChatClient,
        IFaqKnowledgeService faqKnowledgeService,
        IProductAssistantService productAssistantService)
    {
        _anthropicChatClient = anthropicChatClient;
        _faqKnowledgeService = faqKnowledgeService;
        _productAssistantService = productAssistantService;
    }

    public async Task<AiChatResponseDto> ChatAsync(AiChatRequestDto request)
    {
        var message = request.Message?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(message))
            return new AiChatResponseDto { Answer = "Please enter a message." };

        var wantsFaq = LooksLikeFaqQuestion(message);
        var wantsProducts = LooksLikeProductQuestion(message);

        var faqChunks = wantsFaq
            ? await _faqKnowledgeService.SearchRelevantChunksAsync(message)
            : [];

        var products = wantsProducts
            ? await _productAssistantService.FindRelevantProductsAsync(message)
            : [];

        var systemPrompt = BuildSystemPrompt();
        var userPrompt = BuildUserPrompt(message, faqChunks, products);

        var answer = await _anthropicChatClient.GetCompletionAsync(systemPrompt, userPrompt);

        var sources = faqChunks
            .Select(x => new AiSourceDto
            {
                Type = "document",
                Title = x.Title,
                Reference = x.Source
            })
            .ToList();

        return new AiChatResponseDto
        {
            Answer = answer,
            SuggestedProducts = products,
            Sources = sources
        };
    }

    private static bool LooksLikeFaqQuestion(string message)
    {
        var faqWords = new[]
        {
            "return", "refund", "exchange", "shipping", "delivery", "policy", "faq"
        };

        return faqWords.Any(word => message.Contains(word, StringComparison.OrdinalIgnoreCase));
    }

    private static bool LooksLikeProductQuestion(string message)
    {
        var productWords = new[]
        {
            "shoe", "shoes", "gym", "running", "football", "cricket", "product", "buy", "recommend"
        };

        return productWords.Any(word => message.Contains(word, StringComparison.OrdinalIgnoreCase));
    }

    private static string BuildSystemPrompt()
    {
        return """
You are Fitin's shopping assistant.

Rules:
- Use only the provided context.
- Prefer structured product data for price, category, and product facts.
- Do not invent stock, pricing, return rules, or policies.
- If the provided context is not enough, say that clearly.
- Be concise, helpful, and commerce-focused.
""";
    }

    private static string BuildUserPrompt(
        string message,
        List<AiDocumentChunkDto> faqChunks,
        List<AiSuggestedProductDto> products)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"User question: {message}");
        builder.AppendLine();

        if (products.Count > 0)
        {
            builder.AppendLine("Relevant products:");
            foreach (var product in products)
            {
                builder.AppendLine(
                    $"- {product.Name} | Price: {product.Price} | Category: {product.CategoryName} | {product.ShortDescription}");
            }

            builder.AppendLine();
        }

        if (faqChunks.Count > 0)
        {
            builder.AppendLine("Relevant FAQ/policy context:");
            foreach (var chunk in faqChunks)
            {
                builder.AppendLine($"Source: {chunk.Source}");
                builder.AppendLine(chunk.Content);
                builder.AppendLine();
            }
        }

        builder.AppendLine("Answer the user using only the context above.");
        return builder.ToString();
    }
}