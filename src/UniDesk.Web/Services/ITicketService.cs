using UniDesk.Web.Models;
using UniDesk.Web.DTOs;

namespace UniDesk.Web.Services;

public interface ITicketService
{
    Task<PagedResult<TicketListDto>> GetFilteredTicketsAsync(TicketQueryParameters queryParams);
    Task<Ticket?> GetByIdAsync(int id);
    Task AddAsync(Ticket ticket);

    Task UpdateAsync(Ticket ticket);
}