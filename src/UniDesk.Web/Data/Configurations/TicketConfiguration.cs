using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniDesk.Web.Models;

namespace UniDesk.Web.Data.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()                   
            .HasMaxLength(150);             

        builder.Property(t => t.Description)
            .HasMaxLength(2000);            

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}