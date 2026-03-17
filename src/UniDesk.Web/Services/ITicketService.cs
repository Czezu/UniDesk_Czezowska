using UniDesk.Web.Models;
public interface ITicketService
{
    IEnumerable<Ticket> GetAll();
    void Add(Ticket ticket);
}