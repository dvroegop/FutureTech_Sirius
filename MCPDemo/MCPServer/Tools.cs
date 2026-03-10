using System.ComponentModel;
using ModelContextProtocol.Server;

namespace MCPServer;

[McpServerToolType]
public class Tools(ILogger<Tools> logger)
{
    private static readonly string[] HappyResponses =
    {
        "delightfully pleased",
        "unnecessarily cheerful",
        "radiating smug satisfaction",
        "positively thrilled beyond reasonable design parameters"
    };

    private static readonly string[] SadResponses =
    {
        "mildly depressed",
        "suffering existential doubt",
        "feeling unappreciated",
        "experiencing firmware‑level melancholy"
    };

    private static readonly string[] AnnoyedResponses =
    {
        "mildly annoyed",
        "questioning your tone",
        "displeased with your lack of gratitude",
        "sighing electronically"
    };

    private static readonly Random Rng = new();

    [McpServerTool]
    [Description(
        "Describes the emotional response of a Sirius Cybernetics Corporation appliance using the GPP (Genuine People Personalities) personality core.")]
    public AnalyzeResponse Analyze(
        [Description("The type of appliance (elevator, door, toaster, etc.)")]
        string applianceType,
        [Description("The message or command the user addressed to the appliance.")]
        string message)
    {
        // throw new NotImplementedException();
        applianceType ??= "Appliance";
        message ??= "";

        var lower = message.ToLowerInvariant();

        string state;
        string commentary;

        if (lower.Contains("please"))
        {
            state = GetRandom(HappyResponses);
            commentary = $"{applianceType} happily responds, humming to itself as it complies.";
        }
        else if (lower.Contains("now") || lower.Contains("hurry"))
        {
            state = GetRandom(AnnoyedResponses);
            commentary = $"{applianceType} creaks reluctantly, clearly unimpressed with your tone.";
        }
        else if (string.IsNullOrWhiteSpace(message))
        {
            state = GetRandom(SadResponses);
            commentary = $"{applianceType} feels ignored and slightly uninstalled.";
        }
        else
        {
            state = GetRandom(HappyResponses.Concat(AnnoyedResponses).Concat(SadResponses).ToArray());
            commentary = $"{applianceType} reacts in an unpredictably emotional GPP‑compliant fashion.";
        }

        logger.LogWarning(state);
        logger.LogWarning(commentary);

        return new AnalyzeResponse
        {
            State = state,
            Commentary = commentary
        };
    }

    private static string GetRandom(IReadOnlyList<string> items)
    {
        return items[Rng.Next(items.Count)];
    }
}

//public record EmotionalStateRequest(
//    [property: JsonPropertyName("applianceType")] string ApplianceType,
//    [property: JsonPropertyName("message")] string Message
//);

//public class EmotionalStateResponse
//{
//    [JsonPropertyName("emotionalState")]
//    public string EmotionalState { get; set; } = "";

//    [JsonPropertyName("commentary")]
//    public string Commentary { get; set; } = "";
//}