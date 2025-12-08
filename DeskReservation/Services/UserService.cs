using DeskReservation.DbContext;
using DeskReservation.DTOs;
using DeskReservation.Mappers;
using DeskReservation.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DeskReservation.Migrations;
using BCrypt.Net;

namespace DeskReservation.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _userMapper;

    public UserService(AppDbContext context, IMapper userMapper)
    {
        _context = context;
        _userMapper = userMapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        return _userMapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null) return null;
        
        return _userMapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var user = _userMapper.Map<User>(dto);
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return _userMapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> UpdateUserAsync(int id, CreateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null) return null;
        
        _userMapper.Map(dto, user);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        
        await _context.SaveChangesAsync();
        return _userMapper.Map<UserDto>(user);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null) return false;
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}