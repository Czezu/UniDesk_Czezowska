using UniDesk.Web.Models;
namespace UniDesk.Web.Services;

public class TicketService : ITicketService
{
    private readonly List<Ticket> _tickets = new();

    public IEnumerable<Ticket> GetAll() => _tickets;

    public void Add(Ticket ticket)
    {
        ticket.Id = _tickets.Count > 0 ? _tickets.Max(t => t.Id) + 1 : 1;
        ticket.CreatedAt = DateTime.Now;
        ticket.Status = TicketStatus.New;
        _tickets.Add(ticket);
    }

    public Ticket? GetById(int id)
    {
        return _tickets.FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Ticket> Search(string search)
    {
        if (string.IsNullOrWhiteSpace(search)) return _tickets;

        return _tickets.Where(t => t.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
}