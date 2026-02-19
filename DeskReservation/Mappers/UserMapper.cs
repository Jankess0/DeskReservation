using AutoMapper;
using DeskReservation.DTOs;
using DeskReservation.Models;

namespace DeskReservation.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}