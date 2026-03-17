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

    public IActionResult Index(string search)
    {
        var tickets = string.IsNullOrEmpty(search)
            ? _ticketService.GetAll()
            : _ticketService.Search(search);

        return View(tickets);
    }

    public IActionResult Details(int id)
    {
        var ticket = _ticketService.GetById(id);
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