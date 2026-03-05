using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DeskReservation.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int DeskId { get; set; }
    
    public User User { get; set; }
    public Desk Desk { get; set; }
}