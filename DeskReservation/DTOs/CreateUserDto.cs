using System.ComponentModel.DataAnnotations;

namespace DeskReservation.DTOs;

public class CreateUserDto
{
    
    [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
    [EmailAddress(ErrorMessage = "To nie jest poprawny adres e-mail.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [MinLength(8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Imię jest wymagane.")]
    [MaxLength(50, ErrorMessage = "Imię nie może być dłuższe niż {1} znaków.")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Nazwisko jest wymagane.")]
    [MaxLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż {1} znaków.")]
    public string LastName { get; set; }
}