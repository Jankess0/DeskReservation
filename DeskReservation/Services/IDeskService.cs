using DeskReservation.DTOs;
using DeskReservation.Models;
using DeskReservation.State;
using DeskReservation.Strategy;

namespace DeskReservation.Services;

public interface IDeskService
{
    Task<IEnumerable<DeskDto>> GetAllAsync();
    Task<DeskDto> GetDeskAsync(int id);
    Task<bool> CreateDeskAsync(CreateDeskDto desk);
    Task<bool> UpdateDeskAsync(CreateDeskDto desk, int id);
    Task<bool> DeleteDeskAsync(int id);
    Task<bool> CheckInAsync(int deskId, int userId);
    Task<bool> CheckOutAsync(int deskId);
    Task<IEnumerable<DeskDto>> GetAvailableDesksAsync();

}