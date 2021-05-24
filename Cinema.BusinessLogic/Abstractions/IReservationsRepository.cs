using Cinema.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.BusinessLogic.Abstractions
{
    public interface IReservationsRepository
    {
        IEnumerable<Reservations> GetAll();
        Reservations GetReservationById(Guid Id);

    }
}
