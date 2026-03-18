using UniDesk.Web.Data;
using UniDesk.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace UniDesk.Web.Services;

public class TicketService : ITicketService
{
    private readonly UniDeskDbContext _context;

    public TicketService(UniDeskDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Ticket> GetAll()
    {
        return _context.Tickets.ToList();
    }

    public IEnumerable<Ticket> Search(string search)
    {
        if (string.IsNullOrWhiteSpace(search)) return _context.Tickets.ToList();

        return _context.Tickets
            .Where(t => t.Title.ToLower().Contains(search.ToLower()))
            .ToList();
    }

    public Ticket? GetById(int id)
    {
        return _context.Tickets.FirstOrDefault(t => t.Id == id);
    }

    public void Add(Ticket ticket)
    {

        ticket.CreatedAt = DateTime.Now;
        ticket.Status = TicketStatus.New;

        _context.Tickets.Add(ticket);
        _context.SaveChanges();
    }
}