using AutoMapper;
using DeskReservation.DTOs;
using DeskReservation.Models;

namespace DeskReservation.Mappers;

public class DeskMapper : Profile
{
    public DeskMapper()
    {
        CreateMap<Desk, DeskDto>()
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.DeskType, opt => opt.MapFrom(src => src.IsAdminOnly ? DeskType.Vip.ToString() : DeskType.Standard.ToString()));
        CreateMap<DeskDto, Desk>();
    }
}