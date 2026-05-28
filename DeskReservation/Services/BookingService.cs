using AutoMapper;
using DeskReservation.DbContext;
using DeskReservation.DTOs;
using DeskReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace DeskReservation.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BookingService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookings()
    {
        var bookings = await _context.Bookings.ToListAsync();
        var bookingDtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);
        return bookingDtos;
    }

    public async Task<IEnumerable<BookingDto>> GetBookingByUserId(int id)
    {
        var booking = await _context.Bookings
            .Where(b => b.UserId == id)
            .ToListAsync();
        var bookingDtos = _mapper.Map<IEnumerable<BookingDto>>(booking);
        return bookingDtos;
    }

    public async Task<BookingDto> GetBookingById(int id)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null) throw new KeyNotFoundException($"Booking with ID {id} not found");
        var bookingDto = _mapper.Map<BookingDto>(booking);
        return bookingDto;
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByDeskId(int id)
    {
        var bookings = await _context.Bookings
            .Where(b => b.DeskId == id)
            .ToListAsync();
        var bookingDtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);
        return bookingDtos;
    }
}