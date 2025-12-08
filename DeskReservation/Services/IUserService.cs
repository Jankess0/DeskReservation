using DeskReservation.DTOs;

namespace DeskReservation.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserDto user);
    Task<UserDto?> UpdateUserAsync(int id, CreateUserDto user);
    Task<bool> DeleteUserAsync(int id);
}