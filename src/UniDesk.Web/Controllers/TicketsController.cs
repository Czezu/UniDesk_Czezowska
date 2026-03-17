using Microsoft.AspNetCore.Mvc;
using UniDesk.Web.Services;

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
}