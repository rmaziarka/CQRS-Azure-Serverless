using System;

namespace LibraryCorp.Models
{
    public class Borrow: IAggregate
    {
        public Borrow(ReaderId readerId, CopyId copyId)
        {
            BorrowId = new BorrowId(Guid.NewGuid());
            ReaderId = readerId;
            CopyId = copyId;
            BorrowDate = DateTime.Now;
            ReturnDate = BorrowDate.AddDays(30);
        }
        public BorrowId BorrowId { get; private set; }

        public ReaderId ReaderId { get; private set; }

        public CopyId CopyId { get; private set; }

        public DateTime BorrowDate { get; private set; }

        public DateTime ReturnDate { get; private set; }
    }
}
