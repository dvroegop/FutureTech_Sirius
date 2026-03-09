using System.Text.Json.Serialization;

namespace SCC.Deepthought.Domain
{
    public class RefundData
    {
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("amount")]
        public float Amount { get; set; } = 0.0f;
    }
}
