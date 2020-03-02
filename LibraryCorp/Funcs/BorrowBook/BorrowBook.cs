using System;

namespace LibraryCorp.Funcs.BorrowBook
{
    public class BorrowBook
    {
        public Guid ReaderId { get; set; }

        public Guid ReservationId { get; set; }

        public Guid LibraryId { get; set; }

    }
}
