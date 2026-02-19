using DeskReservation.Models;

namespace DeskReservation.Strategy;

public class AdminStrategy : IReservationStrategy
{
    public bool Validate(Desk desk)
    {
        return true;
    }
}