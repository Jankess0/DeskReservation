using DeskReservation.Models;

namespace DeskReservation.Strategy;

public class EmployeeStrategy : IReservationStrategy
{
    public bool Validate(Desk desk)
    {
        return !desk.IsAdminOnly;
    }
}