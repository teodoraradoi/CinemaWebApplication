using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.DataModel
{
    public class BookingViewModel
    {
        public Movies movie { get; set; }
        public Reservations reservation { get; set; }
        public Tickets ticket { get; set; }
    }
}
