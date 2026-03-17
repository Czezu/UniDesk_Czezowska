using Microsoft.EntityFrameworkCore;
using UniDesk.Web.Models;

namespace UniDesk.Web.Data;

public class UniDeskDbContext : DbContext
{
    public UniDeskDbContext(DbContextOptions<UniDeskDbContext> options) : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UniDeskDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<Ticket>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }
}