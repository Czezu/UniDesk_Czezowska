using Microsoft.EntityFrameworkCore;
using UniDesk.Web.Data;
using UniDesk.Web.Models;
using UniDesk.Web.DTOs;

namespace UniDesk.Web.Services;

public class TicketService : ITicketService
{
    private readonly UniDeskDbContext _context;

    public TicketService(UniDeskDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<TicketListDto>> GetFilteredTicketsAsync(TicketQueryParameters queryParams)
    {
        var query = _context.Tickets.AsNoTracking().AsQueryable();

        if (queryParams.Status.HasValue)
        {
            query = query.Where(t => t.Status == queryParams.Status.Value);
        }

        query = queryParams.SortBy?.ToLower() switch
        {
            "title" => query.OrderBy(t => t.Title),
            "status" => query.OrderBy(t => t.Status),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((queryParams.Page - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Select(t => new TicketListDto
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status.ToString(),
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();

        return new PagedResult<TicketListDto>(items, totalCount, queryParams.Page, queryParams.PageSize);
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(Ticket ticket)
    {
        ticket.Status = TicketStatus.New;
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
    }
}