using DeskReservation.DTOs;
using DeskReservation.Models;
using DeskReservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskReservation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom([FromRoute] int id)
    {
        try
        {
            var room = await _roomService.GetRoomById(id);
            return Ok(room);
        }
        catch (Exception ex)
        {
            return StatusCode(404, new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _roomService.GetAllRooms();
        return Ok(rooms);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto dto)
    {
        var result = await _roomService.CreateRoom(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRoom([FromRoute] int id, [FromBody] CreateRoomDto dto)
    {
        try
        {
            var result = await _roomService.UpdateRoom(dto, id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(404, new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRoom([FromRoute] int id)
    {
        try
        {
            var result = await _roomService.DeleteRoom(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(404, new { error = e.Message });
        }
    }
}