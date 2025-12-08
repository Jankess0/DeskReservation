using DeskReservation.DTOs;
using DeskReservation.Models;
using DeskReservation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskReservation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}", Name = "GetUserById")]
    public async Task<IActionResult> GetUserAsync([FromRoute] int id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto userDto)
    {
        var user = await _userService.CreateUserAsync(userDto);
        return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditUserAsync([FromRoute] int id, [FromBody] CreateUserDto userDto)
    {
        var userToUpdate = await _userService.UpdateUserAsync(id, userDto);
        if (userToUpdate == null) return NotFound();
        return Ok(userToUpdate);
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] int id)
    {
        var userToDelete = await _userService.DeleteUserAsync(id);
        if (!userToDelete) return NotFound();
        return NoContent();
    }
}