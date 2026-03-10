using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MCPServer;

public sealed class AnalyzeResponse
{
    [JsonPropertyName("state")]
    [Description("The appliance's current emotional state/personality manifestation.")]
    public string State { get; set; } = "";

    [JsonPropertyName("commentary")]
    [Description("A short narrative describing the appliance's reaction.")]
    public string Commentary { get; set; } = "";
}