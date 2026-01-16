using DeskReservation.Models;

namespace DeskReservation.Observer;

public interface IObserver
{
    void Update(Desk desk);
}