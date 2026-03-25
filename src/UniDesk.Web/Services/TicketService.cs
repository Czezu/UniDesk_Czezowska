using Microsoft.EntityFrameworkCore;
using UniDesk.Web.Data;
using UniDesk.Web.Models;
using UniDesk.Web.DTOs;
using System.Linq.Expressions;

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

        var columnsSelector = new Dictionary<string, Expression<Func<Ticket, object>>>
        {
            { "title", t => t.Title },
            { "status", t => t.Status },
            { "date", t => t.CreatedAt }
        };

        var sortBy = queryParams.SortBy?.ToLower();

        if (!string.IsNullOrEmpty(sortBy) && columnsSelector.ContainsKey(sortBy))
        {
            query = query.OrderBy(columnsSelector[sortBy]);
        }
        else
        {
            query = query.OrderByDescending(t => t.CreatedAt);
        }

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
        if (ticket.CreatedAt == default)
        {
            ticket.CreatedAt = DateTime.Now;
        }
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }
}