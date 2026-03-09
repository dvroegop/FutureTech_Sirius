using Microsoft.EntityFrameworkCore;
using SCC.Deepthought.AI;
using SCC.Deepthought.Controllers;

namespace SCC.Deepthought.Infrastructure;

public static class ServiceRegistrations
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<ControllerRegistrations>();

        services.AddTransient<ITicketValidatorController, TicketValidatorController>();
        services.AddTransient<ITicketValidator, TicketValidator>();
    }
}