using DeskReservation.DTOs;
using DeskReservation.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskReservation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto dto)
    {
        try
        {
            var token = await _userService.LoginAsync(dto);
            return Ok(new { token }); //zwracamy JSON
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}