using AutoMapper;
using DeskReservation.DbContext;
using DeskReservation.DTOs;
using DeskReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace DeskReservation.Services;

public class FloorService : IFloorService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public FloorService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FloorDtoWithRooms> GetFloorAsync(int id)
    {
        var floor = await _context.Floors
            .Include(f => f.Rooms)
            .FirstOrDefaultAsync(f => f.Id == id);
        
        if (floor == null) throw new KeyNotFoundException();
        
        var floorDto = _mapper.Map<FloorDtoWithRooms>(floor);
        
        return floorDto;
    }

    public async Task<IEnumerable<FloorDto>> GetFloorsAsync()
    {
        var floors = await _context.Floors.ToListAsync();
        var floorDtos = _mapper.Map<IEnumerable<FloorDto>>(floors);
        
        return floorDtos;
    }

    public async Task<bool> CreateFloorAsync(CreateFloorDto dto)
    {
        var floor = _mapper.Map<Floor>(dto);
        _context.Add(floor);
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }

    public async Task<bool> UpdateFloorAsync(CreateFloorDto dto, int id)
    {
        var floor = await _context.Floors.FirstOrDefaultAsync(f => f.Id == id);
        if (floor == null) throw new KeyNotFoundException();
        
        _mapper.Map(dto, floor);
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }

    public async Task<bool> DeleteFloorAsync(int id)
    {
        var floor = await _context.Floors.FirstOrDefaultAsync(f => f.Id == id);
        if (floor == null) throw new KeyNotFoundException();
        
        _context.Remove(floor);
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }
}