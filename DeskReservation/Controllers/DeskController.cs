using System.Security.Claims;
using DeskReservation.DTOs;
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
        try
        {
            var desk = await _deskService.GetDeskAsync(id);
            return Ok(desk);
        }
        catch (Exception ex)
        {
            return StatusCode(404, new { error = ex.Message });
        }
    }

    [HttpGet("availabledesks")]
    public async Task<IActionResult> GetAvailableDesksAsync()
    {
        var desks = await _deskService.GetAvailableDesksAsync();
        return Ok(desks);
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

    [HttpPost]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> CreateDeskAsync([FromBody] CreateDeskDto deskDto)
    {
        var result = await _deskService.CreateDeskAsync(deskDto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> UpdateDeskAsync([FromBody] CreateDeskDto deskDto, [FromRoute] int id)
    {
        var result = await _deskService.UpdateDeskAsync(deskDto, id);
        return Ok(result);
    }

    
    [HttpDelete("{id}")]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> DeleteDeskAsync([FromRoute] int id)
    {
        var result = await _deskService.DeleteDeskAsync(id);
        return Ok(result);
    }
}