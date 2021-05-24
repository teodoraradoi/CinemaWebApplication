using Cinema.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.BusinessLogic.Abstractions
{
    public interface ITicketsRepository
    {
        IEnumerable<Tickets> GetAll();

        Tickets GetTicketsById(Guid Id);

    }
}
