namespace DeskReservation.DTOs;

public class FloorDtoWithRooms
{
    public int Id { get; set; }
    public string FloorNumber { get; set; }
    public ICollection<ListRoomDto> Rooms { get; set; }
}