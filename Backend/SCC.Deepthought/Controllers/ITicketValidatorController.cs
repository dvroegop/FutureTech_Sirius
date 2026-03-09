using SCC.Deepthought.Domain;

namespace SCC.Deepthought.Controllers;

public interface ITicketValidatorController
{
    Task<string> ValidateTicket(TicketSummary ticketSummary);
}