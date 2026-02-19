using System.ComponentModel.DataAnnotations;

namespace DeskReservation.Models;

public class Floor
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public ICollection<Room> Rooms { get; set; } =  new HashSet<Room>();
}