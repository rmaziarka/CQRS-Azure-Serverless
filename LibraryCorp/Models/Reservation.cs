using System;

namespace LibraryCorp.Models
{
    public class Reservation: Aggregate
    {
        public Reservation(string readerId, string copyId)
        {
            ReaderId = readerId;
            CopyId = copyId;
            ReservationDate = DateTime.Now;
            ReleaseDate = ReservationDate.AddDays(1);
            Status = ReservationStatus.Pending;
        }

        private Reservation()
        {

        }

        public void Borrow()
        {
            if (Status != ReservationStatus.Pending)
                throw new InvalidOperationException("You cannot take book - it's expired or already taken.");

            Status = ReservationStatus.Borrowed;
        }

        public string ReaderId { get; private set; }

        public string CopyId { get; private set; }

        public DateTime ReservationDate { get; private set; }

        public DateTime ReleaseDate { get; private set; }

        public ReservationStatus Status { get; private set; }
    }

    public enum ReservationStatus
    {
        Pending, Borrowed, Expired
    }
}