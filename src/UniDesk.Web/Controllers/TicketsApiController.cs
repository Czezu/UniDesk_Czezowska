using Microsoft.AspNetCore.Mvc;
using UniDesk.Web.Services;
using UniDesk.Web.DTOs;

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
    public ActionResult<IEnumerable<TicketReadDto>> GetAll()
    {
        var tickets = _ticketService.GetAll();

        var dtos = tickets.Select(t => new TicketReadDto
        {
            Id = t.Id,
            Title = t.Title,
            Status = t.Status.ToString(),
            CreatedAt = t.CreatedAt
        });

        return Ok(dtos);
    }
}