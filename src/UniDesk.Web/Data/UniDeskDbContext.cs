using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UniDesk.Web.Models;

namespace UniDesk.Web.Data;

public class UniDeskDbContext : DbContext
{
    public UniDeskDbContext(DbContextOptions<UniDeskDbContext> options) : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();
}