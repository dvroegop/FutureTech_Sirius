using SCC.Deepthought.Controllers;
using SCC.Deepthought.Domain;

namespace SCC.Deepthought.Infrastructure;

public class ControllerRegistrations
{
    public IEndpointRouteBuilder RegisterControllers(IEndpointRouteBuilder map)
    {
            map.MapPost("validateTicket", async (ITicketValidatorController wdc, TicketSummary ts) => await wdc.ValidateTicket(ts));


        return map;
    }
}