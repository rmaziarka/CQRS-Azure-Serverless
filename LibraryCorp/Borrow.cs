using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryCorp
{
    public class Borrow: Aggregate
    {
        public Borrow(string readerId, string copyId)
        {
            ReaderId = readerId;
            CopyId = copyId;
            BorrowDate = DateTime.Now;
            ReturnDate = BorrowDate.AddDays(30);
        }

        public string ReaderId { get; private set; }

        public string CopyId { get; private set; }

        public DateTime BorrowDate { get; private set; }

        public DateTime ReturnDate { get; private set; }
    }
}
