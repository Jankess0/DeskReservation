namespace DeskReservation.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int UserId { get; set; }
    public int DeskId { get; set; }
    
}