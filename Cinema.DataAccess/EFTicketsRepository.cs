using Cinema.BusinessLogic.Abstractions;
using Cinema.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinema.DataAccess
{
    public class EFTicketsRepository : ITicketsRepository
    {
        private readonly CinemaDbContext dataContext;

        public EFTicketsRepository(CinemaDbContext context)
        {
            dataContext = context;
        }

        public IEnumerable<Tickets> GetAll()
        {
            return dataContext.Tickets.ToList();
        }

        public Tickets GetTicketsById(Guid id)
        {
            return dataContext.Tickets.Single(ticket => ticket.Id.Equals(id));
        }
    }
}
