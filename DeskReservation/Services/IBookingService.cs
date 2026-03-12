using DeskReservation.DTOs;
using DeskReservation.Models;

namespace DeskReservation.Services;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllBookings();
    Task<BookingDto> GetBookingById(int id);
    Task<IEnumerable<BookingDto>> GetBookingByUserId(int id);
    Task<IEnumerable<BookingDto>> GetBookingsByDeskId(int id);
    
}