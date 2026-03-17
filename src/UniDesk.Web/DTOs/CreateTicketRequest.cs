using System.ComponentModel.DataAnnotations;

namespace UniDesk.Web.DTOs;

public class CreateTicketRequest
{
    [Required(ErrorMessage = "Tytuł jest wymagany")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Tytuł musi mieć od 3 do 100 znaków")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Opis jest wymagany")]
    public string Description { get; set; } = string.Empty;
}
