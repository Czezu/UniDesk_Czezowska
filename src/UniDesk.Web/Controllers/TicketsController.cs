using Microsoft.AspNetCore.Mvc;
using UniDesk.Web.Services;
using UniDesk.Web.Models;
using UniDesk.Web.DTOs;

namespace UniDesk.Web.Controllers;

public class TicketsController : Controller
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public async Task<IActionResult> Index([FromQuery] TicketQueryParameters queryParams)
    {
        var result = await _ticketService.GetFilteredTicketsAsync(queryParams);
        return View(result);
    }

    public async Task<IActionResult> Details(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }
        return View(ticket);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Ticket newTicket)
    {
        if (!ModelState.IsValid)
        {
            return View(newTicket);
        }

        await _ticketService.AddAsync(newTicket);
        return RedirectToAction(nameof(Index));
    }
}