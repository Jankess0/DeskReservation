using AutoMapper;
using DeskReservation.DbContext;
using DeskReservation.DTOs;
using DeskReservation.Models;
using DeskReservation.Observer;
using DeskReservation.State;
using DeskReservation.Strategy;
using Microsoft.EntityFrameworkCore;

namespace DeskReservation.Services;

public class DeskService : IDeskService
{
    private readonly AppDbContext _context;
    private readonly IMapper _deskMapper;
    private IEnumerable<IObserver> _observers;

    private const int CleaningTime = 1;


    public DeskService(AppDbContext context, IMapper deskMapper, IEnumerable<IObserver> observers)
    {
        _context = context;
        _deskMapper = deskMapper;
        _observers = observers;
      
    }

    public async Task<IEnumerable<DeskDto>> GetAllAsync()
    {
        var desks = await _context.Desks.ToListAsync();
        var desksDto = _deskMapper.Map<IEnumerable<DeskDto>>(desks);

        for (int i = 0; i < desks.Count(); i++)
        {
            CheckCleaningProgress(desksDto.ElementAt(i), desks[i]);
        }
        return desksDto;
}

    public async Task<DeskDto> GetDeskAsync(int id)
    {
        var desk = await _context.Desks.FindAsync(id);
        if (desk == null) return null;
        
        var deskDto = _deskMapper.Map<DeskDto>(desk);

        CheckCleaningProgress(deskDto, desk);
        
        return deskDto;
    }

    public async Task<bool> CheckInAsync(int deskId, int userId)
    {
        var desk = await _context.Desks.FindAsync(deskId);
        if (desk == null) throw new KeyNotFoundException("Desk not found");
        
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new KeyNotFoundException("User not found");

        var strategy = GetStrategy(user.Role);

        if (!strategy.Validate(desk))
        {
            throw new UnauthorizedAccessException("No permission to check in desk");
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
        
        var result = await _context.SaveChangesAsync() > 0;
        if (result)
        {
            foreach (var observer in _observers)
            {
                observer.Update(desk);
            }
        }
        return result;
    }

    public async Task<bool> CreateDeskAsync(CreateDeskDto deskDto)
    {
        var desk = _deskMapper.Map<Desk>(deskDto);

        desk.Status = DeskState.Available;
        
        await _context.Desks.AddAsync(desk);
        
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }

    public async Task<bool> DeleteDeskAsync(int deskId)
    {
        var desk = await _context.Desks.FindAsync(deskId);
        if (desk == null) throw new Exception("Desk not found");
        _context.Desks.Remove(desk);
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }

    public async Task<bool> UpdateDeskAsync(CreateDeskDto deskDto, int id)
    {
        var desk = await _context.Desks.FindAsync(id);
        if (desk == null) return false;
        
        _deskMapper.Map(deskDto, desk);
        desk.Status = DeskState.Available;
        
        var result = await _context.SaveChangesAsync() > 0;
        return result;
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

    private void CheckCleaningProgress(DeskDto deskDto, Desk desk)
    {
        if (desk.Status == DeskState.Cleaning)
        {
            var timeElapsed = DateTime.UtcNow - desk.LastStatusChangeDate;
            if (timeElapsed.TotalMinutes >= CleaningTime)
            {
                deskDto.Status = "Available";
            }
            else
            {
                int minutesLeft = CleaningTime - (int)timeElapsed.TotalMinutes;
                deskDto.Status = $"Cleaning {minutesLeft} minutes";
            }
        }
    }
}