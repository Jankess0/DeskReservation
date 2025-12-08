using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReservation.Models;

public class Room
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string RoomNumber { get; set; }
    
    [ForeignKey("Floor")]
    public int FloorId { get; set; }
    public Floor Floor { get; set; }
    
    public ICollection<Desk> Desks { get; set; } = new HashSet<Desk>();
}