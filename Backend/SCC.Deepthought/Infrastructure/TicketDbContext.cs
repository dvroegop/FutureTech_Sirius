using Microsoft.EntityFrameworkCore;
using SCC.Deepthought.Domain;

namespace SCC.Deepthought.Infrastructure;

public class TicketDbContext(DbContextOptions<TicketDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;
}
