using DeskReservation.Models;

namespace DeskReservation.Strategy;

public interface IReservationStrategy
{
    public bool Validate(Desk desk);
}