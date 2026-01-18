namespace DeskReservation.DTOs;

public class CreateDeskDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string DeskType { get; set; }
}