using System.ComponentModel.DataAnnotations;

namespace DeskReservation.DTOs;

public class CreateUserDto
{
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string Lastname { get; set; }
}