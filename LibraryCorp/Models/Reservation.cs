using System;

namespace LibraryCorp.Models
{
    public class Reservation: IAggregate
    {
        public Reservation(ReaderId readerId, CopyId copyId)
        {
            ReservationId = new ReservationId(Guid.NewGuid());
            ReaderId = readerId;
            CopyId = copyId;
            ReservationDate = DateTime.Now;
            ReleaseDate = ReservationDate.AddDays(1);
            Status = ReservationStatus.Pending;
        }

        private Reservation()
        {

        }

        public Borrow Borrow()
        {
            if (Status != ReservationStatus.Pending)
                throw new InvalidOperationException("You cannot take book - it's expired or already taken.");

            Status = ReservationStatus.Borrowed;

            return new Borrow(this.ReaderId, this.CopyId);
        }

        public ReservationId ReservationId { get; private set; }

        public ReaderId ReaderId { get; private set; }

        public CopyId CopyId { get; private set; }

        public DateTime ReservationDate { get; private set; }

        public DateTime ReleaseDate { get; private set; }

        public ReservationStatus Status { get; private set; }
    }

    public enum ReservationStatus
    {
        Pending, Borrowed, Expired
    }
}