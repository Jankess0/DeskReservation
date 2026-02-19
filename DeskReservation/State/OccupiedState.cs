using DeskReservation.Models;

namespace DeskReservation.State;

public class OccupiedState : IDeskState
{
    public void CheckIn(Desk desk)
    {
        throw new Exception("Cannot check in, desk is already occupied.");
    }

    public void CheckOut(Desk desk)
    {
        desk.Status = DeskState.Cleaning;
        desk.LastStatusChangeDate = DateTime.UtcNow;
    }
}