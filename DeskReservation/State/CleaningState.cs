using DeskReservation.Models;

namespace DeskReservation.State;

public class CleaningState : IDeskState
{
    public const int CleaningDuration = 30;
    
    public void CheckIn(Desk desk)
    {
        var timeElapsed = DateTime.UtcNow - desk.LastStatusChangeDate;

        if (timeElapsed.TotalMinutes >= CleaningDuration)
        {
            desk.Status = DeskState.Occupied;
            desk.LastStatusChangeDate = DateTime.UtcNow;;
        }
        else
        {
            var minutesLeft = Math.Ceiling(CleaningDuration - timeElapsed.TotalMinutes);
            throw new Exception($"Desk is under cleaning {minutesLeft} minutes.");
        }
    }

    public void CheckOut(Desk desk)
    {
        throw new Exception("You have already checked out the desk.");
    }
}