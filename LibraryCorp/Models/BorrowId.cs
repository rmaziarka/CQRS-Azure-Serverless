using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryCorp.Models
{
    public class BorrowId: TypedIdValueBase
    {
        public BorrowId(Guid value) : base(value)
        {
        }
    }
}
