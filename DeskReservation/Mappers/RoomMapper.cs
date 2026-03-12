using AutoMapper;
using DeskReservation.DTOs;
using DeskReservation.Models;

namespace DeskReservation.Mappers;

public class RoomMapper : Profile
{
    public RoomMapper()
    {
        CreateMap<Room, RoomDtoWithDesks>()
            .ForMember(dest => dest.FloorNumber, opt => opt.MapFrom(src => src.Floor.FloorNumber));
        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.FloorNumber, opt => opt.MapFrom(src => src.Floor.FloorNumber));
        CreateMap<CreateRoomDto, Room>();
    }
    
}