using SCC.Deepthought.AI;
using SCC.Deepthought.Domain;

namespace SCC.Deepthought.Controllers
{
    public class TicketValidatorController(ITicketValidator generator, ILogger<TicketValidatorController> logger) : ITicketValidatorController
    {
        public async Task<string> ValidateTicket(TicketSummary ticketSummary)
        {
            logger.LogInformation("Received request to get validate the ticket. Calling AI");
            var response = await generator.ValidateTicketWithToolsAsync(ticketSummary);
            logger.LogInformation("Received ticket validation from AI: {Response}", response);
            return response;
        }
    }
}
