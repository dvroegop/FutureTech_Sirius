namespace SCC.Deepthought.Domain;

public class TicketSummary
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public TicketType Type { get; set; }
    public string Description { get; set; } = string.Empty;
}