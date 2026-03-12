using AutoMapper;
using DeskReservation.DTOs;
using DeskReservation.Models;

namespace DeskReservation.Mappers;

public class FloorMapper : Profile
{
    public FloorMapper()
    {
        CreateMap<Floor, FloorDtoWithRooms>();
        CreateMap<FloorDtoWithRooms, Floor>();
        CreateMap<FloorDto, Floor>();
        CreateMap<Floor, FloorDto>();
        CreateMap<Floor, CreateFloorDto>();
        CreateMap<CreateFloorDto, Floor>();
        CreateMap<Room, ListRoomDto>();
    }
}