using UniDesk.Web.Models;

public interface ITicketService
{
    IEnumerable<Ticket> GetAll();
    void Add(Ticket ticket);
    Ticket? GetById(int id);
    IEnumerable<Ticket> Search(string search);
}