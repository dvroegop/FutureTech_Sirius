namespace SCC.Deepthought.Domain;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Planet { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public float Importance { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];
}
