using DeskReservation.Models;

namespace DeskReservation.State;

public interface IDeskState
{
    void CheckIn(Desk desk);
    void CheckOut(Desk desk);
}