using UniDesk.Web.Models;
namespace UniDesk.Web.Services;

public class TicketService : ITicketService
{
    private readonly List<Ticket> _tickets = new();
    public IEnumerable<Ticket> GetAll() => _tickets;
    public void Add(Ticket ticket)
    {
        ticket.Id = _tickets.Count + 1;
        ticket.CreatedAt = DateTime.Now;
        _tickets.Add(ticket);
    }
}