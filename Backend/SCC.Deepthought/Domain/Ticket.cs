namespace SCC.Deepthought.Domain;

public enum TicketType
{
    Bug,
    CatastrophicFailure,
    DeadlyMisuse,
    Annoyance
}

public class Ticket
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public TicketType Type { get; set; }
    public string Description { get; set; } = string.Empty;

    public Customer? Customer { get; set; }
}