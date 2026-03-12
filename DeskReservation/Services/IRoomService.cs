using DeskReservation.DTOs;

namespace DeskReservation.Services;

public interface IRoomService
{
    Task<RoomDtoWithDesks> GetRoomById(int id);
    Task<IEnumerable<RoomDto>> GetAllRooms();
    Task<bool> CreateRoom(CreateRoomDto dto);
    Task<bool> UpdateRoom(CreateRoomDto dto, int id);
    Task<bool> DeleteRoom(int id);
}