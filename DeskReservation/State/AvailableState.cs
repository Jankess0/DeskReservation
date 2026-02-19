using DeskReservation.Models;

namespace DeskReservation.State;

public class AvailableState : IDeskState
{
    public void CheckIn(Desk desk)
    {
        desk.Status = DeskState.Occupied;
    }

    public void CheckOut(Desk desk)
    {
        throw new Exception("Cannot check out availble desk.");
    }
}