using DeskReservation.DbContext;
using DeskReservation.DTOs;
using DeskReservation.Mappers;
using DeskReservation.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DeskReservation.Migrations;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DeskReservation.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _userMapper;
    private readonly IConfiguration  _configuration;

    public UserService(AppDbContext context, IMapper userMapper, IConfiguration configuration)
    {
        _context = context;
        _userMapper = userMapper;
        _configuration = configuration;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        return _userMapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null) return null;
        
        return _userMapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var user = _userMapper.Map<User>(dto);
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return _userMapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> UpdateUserAsync(int id, CreateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null) return null;
        
        _userMapper.Map(dto, user);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        
        await _context.SaveChangesAsync();
        return _userMapper.Map<UserDto>(user);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null) return false;
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == dto.Email);
        if (user == null) throw new Exception("Invalid email or password");
        
        bool ispassValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        
        if (!ispassValid) throw new Exception("Invalid email or password");
        
        var token = GenerateJwtToken(user);
        return token;
    }

    public string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        
        //Claims - dane uzytkwonika zaszyte w tokenie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };
        
        //konfiguracja podpisu 
        var creds =  new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        //tworzenie Tokena
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(int.Parse(jwtSettings["ExpireDays"])),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}