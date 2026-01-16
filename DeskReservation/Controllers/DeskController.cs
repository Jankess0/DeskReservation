using System.Security.Claims;
using DeskReservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskReservation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DeskController : ControllerBase
{
    private readonly IDeskService _deskService;

    public DeskController(IDeskService deskService)
    {
        _deskService = deskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDesksAsync()
    {
        var desks = await _deskService.GetAllAsync();

        return Ok(desks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeskAsync([FromRoute] int id)
    {
        var desk = await _deskService.GetDeskAsync(id);
        return Ok(desk);
    }

    [HttpPost("{id}/checkin")]
    public async Task<IActionResult> CheckIn([FromRoute] int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token Error");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return BadRequest("Token Error");

            await _deskService.CheckInAsync(id, userId);
            return Ok(new { message = "Check In Success" });
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("permission"))
            {
                return StatusCode(403, new { error = ex.Message });
            }
            
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("{id}/checkout")]
    public async Task<IActionResult> CheckOut([FromRoute] int id)
    {
        try
        {
            await _deskService.CheckOutAsync(id);
            return Ok(new { message = "Check Out Success" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}