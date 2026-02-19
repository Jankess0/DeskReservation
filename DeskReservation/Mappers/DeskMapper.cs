using AutoMapper;
using DeskReservation.DTOs;
using DeskReservation.Models;

namespace DeskReservation.Mappers;

public class DeskMapper : Profile
{
    public DeskMapper()
    {
        CreateMap<Desk, DeskDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.DeskType,
                opt => opt.MapFrom(src => src.IsAdminOnly ? DeskType.Vip.ToString() : DeskType.Standard.ToString()));
        CreateMap<DeskDto, Desk>();
        CreateMap<CreateDeskDto, Desk>()
            .ForMember(dest => dest.IsAdminOnly,
                opt => opt.MapFrom(src => src.DeskType == DeskType.Vip.ToString() ? true : false));
    }
}