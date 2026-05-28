using System.ComponentModel.DataAnnotations;

namespace DeskReservation.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Błędny format adresu email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Hasło jest wymagane")]
    public string Password { get; set; }
}