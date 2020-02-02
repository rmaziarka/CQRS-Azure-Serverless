using System;
using System.Collections.Generic;

namespace LibraryCorp
{
    public class Reservation
    {
        public Reservation(string readerId, string copyId)
        {
            ReaderId = readerId;
            CopyId = copyId;
            ReservationDate = DateTime.Now;
            ReleaseDate = ReservationDate.AddDay(1);
        }

        public string ReaderId { get; private set; }

        public string CopyId { get; private set; }

        public DateTime ReservationDate { get; private set; }

        public DateTime ReleaseDate { get; private set; }
    }
}