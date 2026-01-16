using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReservation.Models;

public class Desk
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string? Description { get; set; }

    [Required]
    public bool IsAdminOnly;
    
    public DateTime LastStatusChangeDate { get; set; }

    // [ForeignKey("Room")]
    // public int RoomId { get; set; }
    // public Room Room { get; set; }
}