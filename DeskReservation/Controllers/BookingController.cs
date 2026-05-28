using DeskReservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskReservation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetBookings()
    {
        var result = await _bookingService.GetAllBookings();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        var result = await _bookingService.GetBookingById(id);
        return Ok(result);
    }

    [HttpGet("byDeskId/{deskId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetBookingByDeskId(int deskId)
    {
        var result = await _bookingService.GetBookingsByDeskId(deskId);
        return Ok(result);
    }

    [HttpGet("byUserId/{userId}")]
    public async Task<IActionResult> GetBookingByUserId(int userId)
    {
        var result = await _bookingService.GetBookingByUserId(userId);
        return Ok(result);
    }
}