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
    public string LastName { get; set; }

    public UserRole Role { get; set; } = UserRole.User;
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

}