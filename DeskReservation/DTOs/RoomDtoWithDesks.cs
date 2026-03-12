namespace DeskReservation.DTOs;

public class RoomDtoWithDesks
{
    public int Id { get; set; }
    public string RoomNumber { get; set; }
    public string FloorNumber { get; set; }
    public IEnumerable<DeskDto> Desks { get; set; }
    
}