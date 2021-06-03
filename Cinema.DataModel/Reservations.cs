using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.DataModel
{
    public class Reservations
    {
        public Guid Id { get; set; }
        public virtual Guid MovieId { get; set; }
        public virtual Guid TicketId { get; set; }
        public virtual string UserId { get; set; }
        public int NumberOfSeats { get; set; }
    }
}
