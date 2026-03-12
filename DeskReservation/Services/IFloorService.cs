using DeskReservation.DTOs;

namespace DeskReservation.Services;

public interface IFloorService
{
    Task<FloorDtoWithRooms> GetFloorAsync(int id);
    Task<IEnumerable<FloorDto>> GetFloorsAsync();
    Task<bool> CreateFloorAsync(CreateFloorDto floor);
    Task<bool> UpdateFloorAsync(CreateFloorDto floor, int id);
    Task<bool> DeleteFloorAsync(int id);
}