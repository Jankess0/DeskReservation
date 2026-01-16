using AutoMapper;
using DeskReservation.DbContext;
using DeskReservation.DTOs;
using DeskReservation.Models;
using DeskReservation.State;
using DeskReservation.Strategy;
using Microsoft.EntityFrameworkCore;

namespace DeskReservation.Services;

public class DeskService : IDeskService
{
    private readonly AppDbContext _context;
    private readonly IMapper _deskMapper;



    public DeskService(AppDbContext context, IMapper deskMapper)
    {
        _context = context;
        _deskMapper = deskMapper;
    }

    public async Task<IEnumerable<DeskDto>> GetAllAsync()
    {
        var desks = await _context.Desks.ToListAsync();
        return _deskMapper.Map<IEnumerable<DeskDto>>(desks);
    }

    public async Task<DeskDto> GetDeskAsync(int id)
    {
        var desk = await _context.Desks.FindAsync(id);
        if (desk == null) return null;
        
        var deskDto = _deskMapper.Map<DeskDto>(desk);

        if (desk.Status == DeskState.Cleaning)
        {
            var timeElapsed = DateTime.UtcNow - desk.LastStatusChangeDate;
            if (timeElapsed.TotalMinutes >= 30)
            {
                deskDto.State = "Available";
            }
            else
            {
                int minutesLeft = 30 - (int)timeElapsed.TotalMinutes;
                deskDto.State = $"Cleaning {minutesLeft} minutes";
            }
        }
        return deskDto;
    }

    public async Task<bool> CheckInAsync(int deskId, int userId)
    {
        var desk = await _context.Desks.FindAsync(deskId);
        if (desk == null) throw new Exception("Desk not found");
        
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new Exception("User not found");

        var strategy = GetStrategy(user.Role);

        if (!strategy.Validate(desk))
        {
            throw new Exception("No permission to check in desk");
        }

        var state = GetState(desk.Status);
        state.CheckIn(desk);
        
        return await _context.SaveChangesAsync() > 0;

    }

    public async Task<bool> CheckOutAsync(int deskId)
    {
        var desk = await _context.Desks.FindAsync(deskId);
        if (desk == null) throw new Exception("Desk not found");
        
        var state = GetState(desk.Status);
        state.CheckOut(desk);
        
        return await _context.SaveChangesAsync() > 0;
    }

    private IReservationStrategy GetStrategy(UserRole userRole)
    {
        if (userRole == UserRole.Admin) return new AdminStrategy();
        return new EmployeeStrategy();
    }

    private IDeskState GetState(DeskState deskState)
    {
        return deskState switch
        {
            DeskState.Cleaning => new CleaningState(),
            DeskState.Occupied => new OccupiedState(),
            DeskState.Available => new AvailableState(),
            _ => throw new NotImplementedException($"Stan {deskState} not implemented")
        };
    }
}