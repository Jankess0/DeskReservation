using AutoMapper;
using DeskReservation.DbContext;
using DeskReservation.DTOs;
using DeskReservation.Mappers;
using DeskReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace DeskReservation.Services;

public class RoomService : IRoomService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public RoomService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoomDtoWithDesks> GetRoomById(int id)
    {
        var room = await _context.Rooms
            .Include(r => r.Desks)
            .Include(r => r.Floor)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (room == null) throw new KeyNotFoundException();

        var roomDto = _mapper.Map<RoomDtoWithDesks>(room);
        return roomDto;
    }

    public async Task<IEnumerable<RoomDto>> GetAllRooms()
    {
        var rooms = await _context.Rooms
            .Include(r => r.Floor)
            .ToListAsync();
        
        var roomDtos = _mapper.Map<IEnumerable<RoomDto>>(rooms);
        return roomDtos;
    }

    public async Task<bool> CreateRoom(CreateRoomDto dto)
    {
        var room = _mapper.Map<Room>(dto);
        await _context.Rooms.AddAsync(room);
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }

    public async Task<bool> UpdateRoom(CreateRoomDto dto, int id)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null) throw new KeyNotFoundException();
        _mapper.Map(dto, room);
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }

    public async Task<bool> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null) throw new KeyNotFoundException();
        _context.Rooms.Remove(room);
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }
}