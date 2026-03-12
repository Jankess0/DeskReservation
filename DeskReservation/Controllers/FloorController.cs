using DeskReservation.DTOs;
using DeskReservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskReservation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FloorController : ControllerBase
{
    private readonly IFloorService _floorService;

    public FloorController(IFloorService floorService)
    {
        _floorService = floorService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFloorAsync([FromRoute] int id)
    {
        try
        {
            var floor = await _floorService.GetFloorAsync(id);
            return Ok(floor);
        }
        catch (Exception ex)
        {
            return StatusCode(404, new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetFloorsAsync()
    {
        var floors = await _floorService.GetFloorsAsync();
        return Ok(floors);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateFloorAsync([FromBody] CreateFloorDto dto)
    {
        var result = await _floorService.CreateFloorAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditFloorAsync([FromBody] CreateFloorDto dto, [FromRoute] int id)
    {
        try
        {
            var result =  await _floorService.UpdateFloorAsync(dto, id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(404, new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteFloorAsync([FromRoute] int id)
    {
        try
        {
            var result = await _floorService.DeleteFloorAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(404, new { error = e.Message });
        }
    }
    
}