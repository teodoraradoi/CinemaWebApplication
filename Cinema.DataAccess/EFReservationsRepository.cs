using Cinema.BusinessLogic.Abstractions;
using Cinema.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinema.DataAccess
{
    public class EFReservationsRepository : IReservationsRepository
    {
        private readonly CinemaDbContext dataContext;
        public EFReservationsRepository(CinemaDbContext context)
        {
            dataContext = context;
        }
        public IEnumerable<Reservations> GetAll()
        {
            return dataContext.Reservations.ToList();
        }

        public Reservations GetReservationById(Guid Id)
        {
            return dataContext.Reservations.Single(reservation => reservation.Id.Equals(Id));
        }
    }
}
