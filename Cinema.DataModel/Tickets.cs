using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.DataModel
{
    public class Tickets
    {
        public Guid Id { get; set; }
        public virtual Guid MovieId { get; set; }

        public DateTime DateAndTime { get; set; }
    }
}
