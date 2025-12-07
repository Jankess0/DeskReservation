using System.ComponentModel.DataAnnotations;

namespace DeskReservation.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string Lastname { get; set; }
    
}