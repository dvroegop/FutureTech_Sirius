using SCC.Deepthought.Domain;

namespace SCC.Deepthought.AI;

public interface ITicketValidator
{
    Task<string> ValidateTicket(TicketSummary ticketSummary);
    Task<string> ValidateTicketWithToolsAsync(TicketSummary ticketSummary);
}