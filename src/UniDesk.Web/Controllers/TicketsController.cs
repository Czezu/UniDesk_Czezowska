using Microsoft.AspNetCore.Mvc;
using UniDesk.Web.Services;
using UniDesk.Web.Models;

public class TicketsController : Controller
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public IActionResult Index()
    {
        var tickets = _ticketService.GetAll();
        return View(tickets);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Ticket newTicket)
    {
        if (!ModelState.IsValid)
        {
            return View(newTicket);
        }

        _ticketService.Add(newTicket);
        return RedirectToAction(nameof(Index));
    }
}