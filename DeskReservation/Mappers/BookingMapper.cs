using AutoMapper;
using DeskReservation.DTOs;
using DeskReservation.Models;

namespace DeskReservation.Mappers;

public class BookingMapper : Profile
{
    public BookingMapper()
    {
        CreateMap<Booking, BookingDto>();
    }
}