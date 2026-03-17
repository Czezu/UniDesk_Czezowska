using Microsoft.AspNetCore.Mvc;
using UniDesk.Web.Services;
using UniDesk.Web.Models;
using UniDesk.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace UniDesk.Web.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsApiController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsApiController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TicketListDto>>> GetAll([FromQuery] TicketQueryParameters queryParams)
    {
        var result = await _ticketService.GetFilteredTicketsAsync(queryParams);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketReadDto>> GetById(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null) return NotFound();

        return Ok(new TicketReadDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Status = ticket.Status.ToString(),
            CreatedAt = ticket.CreatedAt
        });
    }

    [HttpPost]
    public async Task<ActionResult<TicketReadDto>> Create(CreateTicketRequest request)
    {
        try
        {
            var newTicket = new Ticket
            {
                Title = request.Title,
                Description = request.Description
            };

            await _ticketService.AddAsync(newTicket);

            var dto = new TicketReadDto
            {
                Id = newTicket.Id,
                Title = newTicket.Title,
                Status = newTicket.Status.ToString(),
                CreatedAt = newTicket.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(new
            {
                error = "Wystąpił błąd podczas zapisu do bazy danych.",
                details = ex.Message
            });
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TicketStatus newStatus)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null) return NotFound();

        try
        {
            ticket.Status = newStatus;
            return NoContent();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { error = "Nie udało się zaktualizować statusu bazy danych." });
        }
    }
}